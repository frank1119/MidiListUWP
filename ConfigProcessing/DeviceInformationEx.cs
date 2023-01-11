using Windows.Devices.Enumeration;

namespace MidiUWPRouter
{
    /// <summary>
    /// This is class encapsulates a DeviceInformation and adds a CookedName and a boolean to expose the type (BLE or not).
    /// This is because the properties of a DeviceInformation object are readonly and the
    /// name of the device sometimes has to be derived in a different way
    /// </summary>
    public class DeviceInformationEx
    {
        private readonly string cookedName;
        private readonly bool isBLE;
        private readonly DeviceInformation deviceInformation;

        public string CookedName { get => cookedName; }
        public bool IsBLE => isBLE;
        public DeviceInformation DeviceInformation => deviceInformation;

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        /// <param name="devInfo">The original DeviceInfo object</param>
        /// <param name="cookedName">A derived name</param>
        /// <param name="isBLE">True if this is a Bluetooth LE device</param>
        public DeviceInformationEx(DeviceInformation devInfo, string cookedName, bool isBLE)
        {
            deviceInformation = devInfo;
            this.cookedName = cookedName;
            this.isBLE = isBLE;
        }
    }
}
