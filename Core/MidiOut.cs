using System;
using Windows.Devices.Midi;

namespace MidiUWPRouter.Core
{
    /// <summary>
    /// This is an encapsulation of a UWP MidiOutPort. It adds some extra information.
    /// </summary>
    public class MidiOut
    {
        private IMidiOutPort midiOutPort;

        private readonly string name;
        private readonly string deviceId;
        private readonly bool isBLE;

        public string Name => name;
        public string DeviceId => deviceId;
        public bool IsBLE => isBLE;


        /// <summary>
        /// Constructs a new MidiOut object with some extra info fields
        /// </summary>
        /// <param name="deviceInfo">The DeviceInfoEx holding the info</param>
        public MidiOut(DeviceInformationEx deviceInfo)
        {
            deviceId = deviceInfo.DeviceInformation.Id;
            name = deviceInfo.CookedName;
            isBLE = deviceInfo.IsBLE;
        }

        /// <summary>
        /// Opens the MIDI out device
        /// </summary>
        /// <exception cref="MidiRouterException"></exception>
        public void Open()
        {
            midiOutPort = MidiOutPort.FromIdAsync(deviceId).AsTask().Result;
            if (midiOutPort == null)
            {
                throw new MidiRouterException("MidiOut port {0} with id {1} could not be opened", name, deviceId);
            }
        }

        /// <summary>
        /// Closes the object
        /// </summary>
        public void Close()
        {
            try
            {
                if (midiOutPort != null)
                {
                    midiOutPort.Dispose();
                }
            }
            catch { }
            midiOutPort = null;
        }

        /// <summary>
        /// Sends a MIDI message
        /// </summary>
        /// <param name="midiMessage">The MIDI message to send</param>
        public void SendMessage(IMidiMessage midiMessage)
        {
            midiOutPort.SendMessage(midiMessage);
        }

    }
}
