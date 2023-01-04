using System;
using Windows.Devices.Midi;
using Windows.Foundation;

namespace MidiUWPRouter.Core
{
    /// <summary>
    /// This is an encapsulation of a UWP MidiInPort. It adds some extra information and the
    /// Open() method is somewhat enhanced to accommodate BLE.
    /// </summary>
    public class MidiIn
    {
        private readonly string name;
        private readonly string deviceId;
        private readonly bool isBLE;
        private MidiInPort midiInPort;

        public string Name => name;
        public string DeviceId => deviceId;

        public bool IsBLE => isBLE;

        // No relay: Direct patching through
        public event TypedEventHandler<MidiInPort, MidiMessageReceivedEventArgs> MidiInEvent
        {
            add { midiInPort.MessageReceived += value; }
            remove { midiInPort.MessageReceived -= value; }
        }

        /// <summary>
        /// Reports the progress of opening a MidiInPort, but only when there are retries.
        /// -1 indicates a successful open. -2 indicates a failure
        /// </summary>
        /// <param name="retriesLeft">Number of retries left. -1 indicates a successful open. -2 indicates a failure</param>
        public delegate void MidiInOpenProgressEventHandler(int retriesLeft);
        public event MidiInOpenProgressEventHandler MidiInOpenProgress;

        /// <summary>
        /// Constructs a new MidiIn object with some extra info fields
        /// </summary>
        /// <param name="deviceInfo">The DeviceInfoEx holding the info</param>
        public MidiIn(DeviceInformationEx deviceInfo)
        {
            deviceId = deviceInfo.DeviceInformation.Id;
            name = deviceInfo.CookedName;
            isBLE = deviceInfo.IsBLE;
        }

        /// <summary>
        /// Opens the MIDI out device. For BLE devices this might take some time. Also opening might fail a few times 
        /// before the opening succeeds. Therefore some reporting using MidiInOpenProgress is provided.
        /// </summary>
        /// <param name="retries">An optional parameter indicting the maximum number of retries</param>
        /// <exception cref="MidiRouterException"></exception>
        public void Open(int retries = 30)
        {
            int i = 0;
            while (midiInPort == null && (i++ < retries))
            {
                IAsyncOperation<MidiInPort> asyncOp = MidiInPort.FromIdAsync(deviceId);
                asyncOp.AsTask().Wait(300);

                if (asyncOp.Status == AsyncStatus.Completed)
                {
                    midiInPort = asyncOp.GetResults();
                    if (midiInPort != null)
                        break;
                }
                else
                {
                    MidiInOpenProgress?.Invoke(retries - i);
                    asyncOp.Cancel();
                }
            }

            if (midiInPort == null)
            {
                if (i > 1)
                    MidiInOpenProgress?.Invoke(-2);  // Failure

                throw new MidiRouterException("MidiIn port {0} with id {1} could not be opened", name, deviceId);
            }
            else
            {
                if (i > 1)
                    MidiInOpenProgress?.Invoke(-1);  // Success
            }
        }

        /// <summary>
        /// Closes the object
        /// </summary>
        public void Close()
        {
            try
            {
                if (midiInPort != null)
                {
                    midiInPort.Dispose();
                }
            }
            catch { }
            midiInPort = null;
        }

    }
}
