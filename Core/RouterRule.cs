using System;
using Windows.Devices.Midi;
using static MidiUWPRouter.Core.MidiIn;

namespace MidiUWPRouter.Core
{
    /// <summary>
    /// A RouterRule is the connection between one midi-input and one midi-output.
    /// All messages from the input goes to the output.
    /// 
    /// Although a RouterRule can only handle one input and one output, it is possible 
    /// to have a midi-input/midi-output participate in more than one rule.
    /// </summary>
    internal class RouterRule : IDisposable
    {
        private readonly string routeName;
        private MidiIn midiIn;
        private MidiOut midiOut;

        private bool disposedValue;

        public string SourceName => midiIn.Name;
        public string DestinationName => midiOut.Name;
        public string RouteName => routeName;
        public bool IsMidiInBLE { get { return midiIn.IsBLE; } }

        /// <summary>
        /// Reports the progress of opening a MidiInPort, but only when there are retries.
        /// -1 indicates a successful open. -2 indicates a failure. These are purely
        /// reported for cosmetic purposes.
        /// </summary>
        public event MidiInOpenProgressEventHandler MidiInOpenProgress
        {
            add { midiIn.MidiInOpenProgress += value; }
            remove { midiIn.MidiInOpenProgress -= value; }
        }

        /// <summary>
        /// Event handler. Relays the incoming MIDI message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MidiInPort_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            IMidiMessage receivedMidiMessage = args.Message;
            try
            {
                midiOut.SendMessage(receivedMidiMessage);
            }
            catch
            { }
        }

        /// <summary>
        /// Instantiate a RouterRule. This is only caching of the parts
        /// </summary>
        /// <param name="routeName">Name of the route</param>
        /// <param name="midiIn">The MidiIn source</param>
        /// <param name="midiOut">The MidiOut destination</param>
        /// <exception cref="MidiRouterException"></exception>
        public RouterRule(string routeName, MidiIn midiIn, MidiOut midiOut)
        {
            if (midiIn == null || midiOut == null)
            {
                throw new MidiRouterException("Instantiating RouterRule {0}: Input or output cannot be null",routeName);
            }

            this.routeName = routeName;
            this.midiIn = midiIn;
            this.midiOut = midiOut;
        }

        /// <summary>
        /// Opens the MidiIn source and MidiOut destination. Registers an eventhandler to
        /// receive MIDI messages
        /// </summary>
        /// <exception cref="MidiRouterException"></exception>
        public void Open()
        {
            if (midiIn == null || midiOut == null)
                throw new MidiRouterException("Route {0} has been disposed", RouteName);

            midiOut.Open();
            midiIn.Open(30);
            midiIn.MidiInEvent += MidiInPort_MessageReceived;
        }

        /// <summary>
        /// Closes all the MidiIn source and MidiOut destination 
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (midiIn != null)
                    {
                        try
                        {
                            midiIn.MidiInEvent -= MidiInPort_MessageReceived;
                        }
                        catch { }

                        midiIn.Close();
                        midiIn = null;
                    }
                    if (midiOut != null)
                    {
                        midiOut.Close();
                        midiOut = null;
                    }

                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~RouterRule()
        {
            //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
