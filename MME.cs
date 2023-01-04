using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MidiListUWP
{
    internal class MME
    {


        [DllImport("winmm.dll")]
        public static extern int midiOutGetNumDevs();
        [DllImport("winmm.dll")]
        public static extern int midiInGetNumDevs();


        [DllImport("winmm.dll")]
        static extern int midiInGetDevCaps(int deviceIndex, ref MIDIINCAPS caps, int sizeOfMidiInCaps);

        [DllImport("winmm.dll")] 
        static extern int midiOutGetDevCaps(int deviceIndex, ref MIDIOUTCAPS caps, int uSize);


        [StructLayout(LayoutKind.Sequential)]
        private struct MIDIINCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;     // MMVERSION
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public uint dwSupport;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MIDIOUTCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;     //MMVERSION
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public ushort wTechnology;
            public short wVoices;
            public short wNotes;
            public ushort wChannelMask;
            public uint dwSupport;
        }

        public static string GetInName(int deviceIndex)
        {
            MIDIINCAPS _caps = new MIDIINCAPS();
            midiInGetDevCaps(deviceIndex, ref _caps, Marshal.SizeOf(typeof(MIDIINCAPS)));
            return _caps.szPname;
        }

        public static string GetOutName(int deviceIndex)
        {
            MIDIOUTCAPS _caps = new MIDIOUTCAPS();
            midiOutGetDevCaps(deviceIndex, ref _caps, Marshal.SizeOf(typeof(MIDIOUTCAPS)));
            return _caps.szPname;
        }

    }
}
