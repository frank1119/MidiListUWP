using System;

namespace MidiUWPRouter
{
    internal class MidiRouterException : Exception
    {
        public MidiRouterException(string message) : base(message)
        {

        }

        public MidiRouterException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        public MidiRouterException(string format, object arg0) : base(string.Format(format, arg0))
        {
        }

        public MidiRouterException(string format, object arg0, object arg1) : base(string.Format(format, arg0, arg1))
        {
        }

        public MidiRouterException(string format, object arg0, object arg1, object arg2) : base(string.Format(format, arg0, arg1, arg2))
        {
        }

    }
}
