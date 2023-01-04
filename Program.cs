using MidiUWPRouter.ConfigProcessing;
using MidiUWPRouter.Core;
using System;
using System.IO;
//using System.Windows.Forms;

namespace MidiUWPRouter
{
    internal partial class Program
    {
        static void ShowDevices()
        {
            var sortedMidiIn = ConfigReader.GetSortedList(ConfigReader.GetMidiInDevices(), true);
            var sortedMidiOut = ConfigReader.GetSortedList(ConfigReader.GetMidiOutDevices(), false);

            Console.WriteLine("Input devices:");
            Console.WriteLine("==============");
            foreach (var p in sortedMidiIn)
            {
                Console.WriteLine("CookedName : {0}\r\nRealName   : {1}\r\nDevice ID  : {2}\r\n", p.CookedName, p.DeviceInformation.Name, p.DeviceInformation.Properties["System.Devices.DeviceInstanceId"].ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Output devices:");
            Console.WriteLine("===============");
            foreach (var p in sortedMidiOut)
            {
                Console.WriteLine("CookedName : {0}\r\nRealName   : {1}\r\nDevice ID  : {2}\r\n", p.CookedName, p.DeviceInformation.Name, p.DeviceInformation.Properties["System.Devices.DeviceInstanceId"].ToString());
            }
        }

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            if (args.Length == 0)
            {
                ShowDevices();
                return;
            }

            /*
               For the future :-)
               Control.CheckForIllegalCrossThreadCalls = true;
               Form1 frm = new Form1();
               Application.Run(frm);
            */


            Router.RouteInstalledEvent += Router_RouteInstalledEvent;
            Router.RouterSkipEvent += Router_RouteSkipEvent;
            Router.AliasFoundEvent += Router_AliasFoundEvent;
            Router.RouterErrorEvent += Router_RouterErrorEvent;
            Router.MidiInOpenProgress += Router_MidiPortOpenProgress;
            Router.RouteReportMidiInTypeEvent += Router_RouteReportMidiInTypeEvent;
            try
            {
                Router.StartRouter(args[0]);

                Console.WriteLine("\r\nEnter 'quit' to exit.");
                string resp = "";
                while (resp?.ToLower() != "quit")
                    resp = Console.ReadLine();

                Router.StopRouter();
            }
            catch (IOException ioex)
            {
                Console.WriteLine("I/O exception: {0}", ioex.Message);
            }
            catch (MidiRouterException mrex)
            {
                Console.WriteLine("MIDI router exception: {0}", mrex.Message);
            }
        }

        private static void Router_RouteReportMidiInTypeEvent(string routeName, string sourceName, string destinationName, bool isBLE)
        {
            if (isBLE)
                Console.Write("Route {0}. Going to open BLE-Source {1} ", routeName, sourceName);
        }

        private static void Router_MidiPortOpenProgress(int retriesLeft)
        {
            switch (retriesLeft)
            {
                case -1:
                    Console.WriteLine(" Done.");
                    break;
                case -2:
                    Console.WriteLine(" Failed.");
                    break;
                default:
                    Console.Write(".");
                    break;
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private static void Router_RouterErrorEvent(string configFileName, Exception e, string routeName, string sourceName, string destinationName)
        {
            Console.WriteLine("Error installing {0}. Source {1} destination {2} {3}", routeName, sourceName, destinationName, e.Message);
        }

        private static void Router_AliasFoundEvent(string type, string deviceName, string aliasName)
        {
            Console.WriteLine("Alias found {0} {1} -> {2}", type, deviceName, aliasName);
        }

        private static void Router_RouteSkipEvent(string routeName, string sourceName, string destinationName)
        {
            Console.WriteLine("Skipping {0}. Source {1} destination {2}", routeName, sourceName, destinationName);
        }

        private static void Router_RouteInstalledEvent(string routeName, string sourceName, string destinationName)
        {
            Console.WriteLine("Installed {0}. Source {1} destination {2}", routeName, sourceName, destinationName);
        }
    }
}
