using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MidiUWPRouter;

namespace MidiUWPRouter.MultiForm
{

    internal class NativeMethods
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }

    // The class that handles the creation of the application windows 
    class MFDriver : ApplicationContext
    {

        public static int FormCount { get; private set; } = 0;

        public static FrmDevices FrmDev { get; private set; }

        private static MFDriver Context { get; } = new MFDriver();

        public static void RegisterModelessForm(Form newForm)
        {
            newForm.Closed += new EventHandler(OnFormClosed);
            newForm.Closing += new CancelEventHandler(OnFormClosing);
            FormCount++;
        }

        public static void RegisterBackgroundform(Form newForm)
        {
            newForm.Closed += new EventHandler(OnHiddenFormClosed);
            newForm.Closing += new CancelEventHandler(OnHiddenFormClosing);
        }

        private MFDriver()
        {
            FormCount = 0;

            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // When the application is exiting, write the application data to the 
            // user file and close it.

        }

        private static void OnFormClosing(object sender, CancelEventArgs e)
        {
            // When a form is closing, remember the form position so it 
            // can be saved in the user data file. 
        }

        private static void OnFormClosed(object sender, EventArgs e)
        {
            // When a form is closed, decrement the count of open forms. 
            // When the count gets to 0, exit the app by calling 
            // ExitThread().

            FormCount--;
            if (FormCount == 0)
            {
                Context.ExitThread();
            }
        }

        private static void OnHiddenFormClosing(object sender, CancelEventArgs e)
        {
            // When a form is closing, remember the form position so it 
            // can be saved in the user data file. 
        }

        private static void OnHiddenFormClosed(object sender, EventArgs e)
        {
            // When a form is closed, decrement the count of open forms. 
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801")]
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG
            Control.CheckForIllegalCrossThreadCalls = true;
#endif


            FrmDev = new FrmDevices(args.Length == 1 ? args[0] : "");
            FrmDev.Show();
            Application.Run(Context);
        }
    }
}
