using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using BridgeRT;
using Windows.Devices.Gpio;

namespace AdapterLib
{
    public sealed class Adapter : IAdapter
    {
        private const uint ERROR_SUCCESS = 0;

        public string Vendor { get; }

        public string AdapterName { get; }

        public string Version { get; }

        public string ExposedAdapterPrefix { get; }

        public string ExposedApplicationName { get; }

        public Guid ExposedApplicationGuid { get; }

        public IList<IAdapterSignal> Signals { get; }

        // GPIO Device Pin-5 Property
        private const int PIN_NUMBER5 = 5;
        private const string PIN_NAME5 = "Pin-5";
        private const string PIN_INTERFACE_HINT5 = "";

        // Pin-5 Property Attribute
        private const string PIN_VALUE_NAME5 = "PinValue";
        private int pinValueData5 = -1;

        // GPIO Device Pin-6 Property
        private const int PIN_NUMBER6 = 6;
        private const string PIN_NAME6 = "Pin-6";
        private const string PIN_INTERFACE_HINT6 = "";

        // Pin-6 Property Attribute
        private const string PIN_VALUE_NAME6 = "PinValue";
        private int pinValueData6 = -1;

        // GPIO Device Pin-12 Property
        private const int PIN_NUMBER12 = 12;
        private const string PIN_NAME12 = "Pin-12";
        private const string PIN_INTERFACE_HINT12 = "";

        // Pin-12 Property Attribute
        private const string PIN_VALUE_NAME12 = "PinValue";
        private int pinValueData12 = -1;

        // GPIO Device Pin-16 Property
        private const int PIN_NUMBER16 = 16;
        private const string PIN_NAME16 = "Pin-16";
        private const string PIN_INTERFACE_HINT16 = "";

        // Pin-16 Property Attribute
        private const string PIN_VALUE_NAME16 = "PinValue";
        private int pinValueData16 = -1;


        private GpioController controller;
        private GpioPin pin5;
        private GpioPin pin6;
        private GpioPin pin12;
        private GpioPin pin16;

        public Adapter()
        {
            Windows.ApplicationModel.Package package = Windows.ApplicationModel.Package.Current;
            Windows.ApplicationModel.PackageId packageId = package.Id;
            Windows.ApplicationModel.PackageVersion versionFromPkg = packageId.Version;

            this.Vendor = "RohitNarayan";
            this.AdapterName = "AllJoynDSBSample";

            // the adapter prefix must be something like "com.mycompany" (only alpha num and dots)
            // it is used by the Device System Bridge as root string for all services and interfaces it exposes
            this.ExposedAdapterPrefix = "com." + this.Vendor.ToLower();
            this.ExposedApplicationGuid = Guid.Parse("{0xeb261659,0x67df,0x4c78,{0x9d,0x30,0xbb,0x5c,0x71,0x51,0x46,0x7d}}");

            if (null != package && null != packageId)
            {
                this.ExposedApplicationName = packageId.Name;
                this.Version = versionFromPkg.Major.ToString() + "." +
                               versionFromPkg.Minor.ToString() + "." +
                               versionFromPkg.Revision.ToString() + "." +
                               versionFromPkg.Build.ToString();
            }
            else
            {
                this.ExposedApplicationName = "DeviceSystemBridge";
                this.Version = "0.0.0.0";
            }

            try
            {
                this.Signals = new List<IAdapterSignal>();
                this.devices = new List<IAdapterDevice>();
                this.signalListeners = new Dictionary<int, IList<SIGNAL_LISTENER_ENTRY>>();
            }
            catch (OutOfMemoryException ex)
            {
                throw new OutOfMemoryException(ex.Message);
            }

            controller = GpioController.GetDefault();
            pin5 = controller.OpenPin(PIN_NUMBER5);           // Open GPIO 5
            pin5.SetDriveMode(GpioPinDriveMode.Output);       // Set the IO direction as output

            pin6 = controller.OpenPin(PIN_NUMBER6);           // Open GPIO 5
            pin6.SetDriveMode(GpioPinDriveMode.Output);       // Set the IO direction as output

            pin12 = controller.OpenPin(PIN_NUMBER12);           // Open GPIO 5
            pin12.SetDriveMode(GpioPinDriveMode.Output);       // Set the IO direction as output

            pin16 = controller.OpenPin(PIN_NUMBER16);           // Open GPIO 5
            pin16.SetDriveMode(GpioPinDriveMode.Output);       // Set the IO direction as output
        }

        public uint SetConfiguration([ReadOnlyArray] byte[] ConfigurationData)
        {
            return ERROR_SUCCESS;
        }

        public uint GetConfiguration(out byte[] ConfigurationDataPtr)
        {
            ConfigurationDataPtr = null;

            return ERROR_SUCCESS;
        }

        public uint Initialize()
        {
            AdapterDevice devicePin5 = new AdapterDevice("Switch 1", "Rohit Narayan", "Testing Model", "0.0.1", "000000001", "Controls Pin 5");
            AdapterDevice devicePin6 = new AdapterDevice("Switch 2", "Rohit Narayan", "Testing Model", "0.0.1", "000000001", "Controls Pin 6");
            AdapterDevice devicePin12 = new AdapterDevice("Switch 3", "Rohit Narayan", "Testing Model", "0.0.1", "000000001", "Controls Pin 12");
            AdapterDevice devicePin16 = new AdapterDevice("Switch 4", "Rohit Narayan", "Testing Model", "0.0.1", "000000001", "Controls Pin 16");

            // Define GPIO Pin-5 as device property. Device contains properties
            AdapterProperty gpioPin_Property5 = new AdapterProperty(PIN_NAME5, PIN_INTERFACE_HINT5);
            // Define and set GPIO Pin-5 value. Device contains properties that have one or more attributes.
            pinValueData5 = (int)pin5.Read();
            AdapterValue pinValueAttr5 = new AdapterValue(PIN_VALUE_NAME5, pinValueData5);
            gpioPin_Property5.Attributes.Add(pinValueAttr5);

            // Define GPIO Pin-6 as device property. Device contains properties
            AdapterProperty gpioPin_Property6 = new AdapterProperty(PIN_NAME6, PIN_INTERFACE_HINT6);
            // Define and set GPIO Pin-6 value. Device contains properties that have one or more attributes.
            pinValueData6 = (int)pin6.Read();
            AdapterValue pinValueAttr6 = new AdapterValue(PIN_VALUE_NAME6, pinValueData6);
            gpioPin_Property6.Attributes.Add(pinValueAttr6);

            // Define GPIO Pin-12 as device property. Device contains properties
            AdapterProperty gpioPin_Property12 = new AdapterProperty(PIN_NAME12, PIN_INTERFACE_HINT12);
            // Define and set GPIO Pin-12 value. Device contains properties that have one or more attributes.
            pinValueData12 = (int)pin12.Read();
            AdapterValue pinValueAttr12 = new AdapterValue(PIN_VALUE_NAME12, pinValueData12);
            gpioPin_Property12.Attributes.Add(pinValueAttr12);

            // Define GPIO Pin-16 as device property. Device contains properties
            AdapterProperty gpioPin_Property16 = new AdapterProperty(PIN_NAME16, PIN_INTERFACE_HINT16);
            // Define and set GPIO Pin-16 value. Device contains properties that have one or more attributes.
            pinValueData16 = (int)pin16.Read();
            AdapterValue pinValueAttr16 = new AdapterValue(PIN_VALUE_NAME16, pinValueData16);
            gpioPin_Property16.Attributes.Add(pinValueAttr16);

            devicePin5.Properties.Add(gpioPin_Property5);

            devicePin6.Properties.Add(gpioPin_Property6);

            devicePin12.Properties.Add(gpioPin_Property12);

            devicePin16.Properties.Add(gpioPin_Property16);

            devices.Add(devicePin5);

            devices.Add(devicePin6);

            devices.Add(devicePin12);

            devices.Add(devicePin16);

            return ERROR_SUCCESS;
        }

        public uint Shutdown()
        {
            throw new NotImplementedException();
        }

        public uint EnumDevices(
            ENUM_DEVICES_OPTIONS Options,
            out IList<IAdapterDevice> DeviceListPtr,
            out IAdapterIoRequest RequestPtr)
        {
            RequestPtr = null;

            try
            {
                DeviceListPtr = new List<IAdapterDevice>(this.devices);
            }
            catch (OutOfMemoryException ex)
            {
                throw new OutOfMemoryException(ex.Message);
            }

            return ERROR_SUCCESS;
        }

        public uint GetProperty(
            IAdapterProperty Property,
            out IAdapterIoRequest RequestPtr)
        {
            RequestPtr = null;

            return ERROR_SUCCESS;
        }

        public uint SetProperty(
            IAdapterProperty Property,
            out IAdapterIoRequest RequestPtr)
        {
            RequestPtr = null;

            return ERROR_SUCCESS;
        }

        public uint GetPropertyValue(
            IAdapterProperty Property,
            string AttributeName,
            out IAdapterValue ValuePtr,
            out IAdapterIoRequest RequestPtr)
        {
            RequestPtr = null;
            IAdapterValue attribute = Property.Attributes.ElementAt<IAdapterValue>(0);
            if (Property.Name == "Pin-5")
            {
                pinValueData5 = (int)pin5.Read();

                attribute.Data = pinValueData5;
            }
            else if (Property.Name == "Pin-6")
            {
                pinValueData6 = (int)pin6.Read();

                attribute.Data = pinValueData6;
            }
            else if (Property.Name == "Pin-12")
            {
                pinValueData12 = (int)pin12.Read();

                attribute.Data = pinValueData12;
            }
            else if (Property.Name == "Pin-16")
            {
                pinValueData16 = (int)pin16.Read();

                attribute.Data = pinValueData16;
            }

            ValuePtr = attribute;

            return ERROR_SUCCESS;
        }

        public uint SetPropertyValue(
            IAdapterProperty Property,
            IAdapterValue Value,
            out IAdapterIoRequest RequestPtr)
        {
            RequestPtr = null;

            IAdapterValue attribute = Property.Attributes.ElementAt<IAdapterValue>(0);
            if (Property.Name == "Pin-5")
            {
                if (Convert.ToInt32(Value.Data) == 1)
                {
                    pinValueData5 = 1;
                    pin5.Write(GpioPinValue.High);
                }
                else
                {
                    pinValueData5 = 0;
                    pin5.Write(GpioPinValue.Low);
                }
            }
            else if (Property.Name == "Pin-6")
            {
                if (Convert.ToInt32(Value.Data) == 1)
                {
                    pinValueData6 = 1;
                    pin6.Write(GpioPinValue.High);
                }
                else
                {
                    pinValueData6 = 0;
                    pin6.Write(GpioPinValue.Low);
                }
            }
            else if (Property.Name == "Pin-12")
            {
                if (Convert.ToInt32(Value.Data) == 1)
                {
                    pinValueData12 = 1;
                    pin12.Write(GpioPinValue.High);
                }
                else
                {
                    pinValueData12 = 0;
                    pin12.Write(GpioPinValue.Low);
                }
            }
            else if (Property.Name == "Pin-16")
            {
                if (Convert.ToInt32(Value.Data) == 1)
                {
                    pinValueData16 = 1;
                    pin16.Write(GpioPinValue.High);
                }
                else
                {
                    pinValueData16 = 0;
                    pin16.Write(GpioPinValue.Low);
                }
            }

            attribute.Data = Value.Data;

            return ERROR_SUCCESS;
        }

        public uint CallMethod(
            IAdapterMethod Method,
            out IAdapterIoRequest RequestPtr)
        {
            RequestPtr = null;

            return ERROR_SUCCESS;
        }

        public uint RegisterSignalListener(
            IAdapterSignal Signal,
            IAdapterSignalListener Listener,
            object ListenerContext)
        {
            return ERROR_SUCCESS;
        }

        public uint UnregisterSignalListener(
            IAdapterSignal Signal,
            IAdapterSignalListener Listener)
        {
            return ERROR_SUCCESS;
        }

        private uint createSignals()
        {
            return ERROR_SUCCESS;
        }

        private struct SIGNAL_LISTENER_ENTRY
        {
            // The signal object
            internal IAdapterSignal Signal;

            // The listener object
            internal IAdapterSignalListener Listener;

            //
            // The listener context that will be
            // passed to the signal handler
            //
            internal object Context;
        }

        // List of Devices
        private IList<IAdapterDevice> devices;

        // A map of signal handle (object's hash code) and related listener entry
        private Dictionary<int, IList<SIGNAL_LISTENER_ENTRY>> signalListeners;
    }
}
