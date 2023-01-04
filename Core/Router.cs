using MidiUWPRouter.ConfigProcessing;
using System;
using System.Collections.Generic;
using static MidiUWPRouter.ConfigProcessing.ConfigReader;
using static MidiUWPRouter.Core.MidiIn;

namespace MidiUWPRouter.Core
{
    /// <summary>
    /// The router has two tasks:
    ///   1. Have the configuration file parsed and after that install the configured router rules
    ///   2. Deinstall all router rules
    /// </summary>
    internal class Router
    {
        private static HashSet<RouterRule> routes = null;

        // Event definitions for reporting progress and errors        
        public delegate void RouterErrorEventHandler(string configFileName, Exception e, string routeName, string sourceName, string destinationName);
        public delegate void RouteInstalledEventHandler(string routeName, string sourceName, string destinationName);
        // Event definition for reporting what type of MidiIn port is being opened: BLE can bve really slow
        public delegate void RouteReportMidiInTypeEventHandler(string routeName, string sourceName, string destinationName, bool isBLE);

        // The events to subscribe to
        public static event RouteReportMidiInTypeEventHandler RouteReportMidiInTypeEvent;
        public static event RouterErrorEventHandler RouterErrorEvent;
        public static event RouteInstalledEventHandler RouteInstalledEvent;
        public static event RouteSkipEventHandler RouterSkipEvent
        {
            add { ConfigReader.RouteSkipEvent += value; }
            remove { ConfigReader.RouteSkipEvent -= value; }
        }
        public static event AliasFoundEventHandler AliasFoundEvent
        {
            add { ConfigReader.AliasFoundEvent += value; }
            remove { ConfigReader.AliasFoundEvent -= value; }
        }

        /// <summary>
        /// Reports the progress of opening a MidiInPort, but only when there are retries.
        /// -1 indicates a successful open. -2 indicates a failure
        /// </summary>
        public static event MidiInOpenProgressEventHandler MidiInOpenProgress;

        /// <summary>
        /// Parses the config and installs the routes
        /// </summary>
        /// <param name="configFileName">The configuration to parse and install</param>
        /// <exception cref="MidiRouterException">Only one router is possible</exception>
        public static void StartRouter(string configFileName)
        {
            if (routes != null)
                throw new MidiRouterException("Only one router can run at a time");

            routes = ParseConfig(configFileName);

            foreach (RouterRule routerRule in routes)
            {
                try
                {
                    RouteReportMidiInTypeEvent?.Invoke(routerRule.RouteName, routerRule.SourceName, routerRule.DestinationName, routerRule.IsMidiInBLE);
                    routerRule.MidiInOpenProgress += RouterRule_MidiPortOpenProgress;
                    routerRule.Open();
                    RouteInstalledEvent?.Invoke(routerRule.RouteName, routerRule.SourceName, routerRule.DestinationName);
                }
                catch (Exception e)
                {
                    routerRule.Close();
                    RouterErrorEvent?.Invoke(configFileName, e, routerRule.RouteName, routerRule.SourceName, routerRule.DestinationName);
                }
                finally
                {
                    routerRule.MidiInOpenProgress -= RouterRule_MidiPortOpenProgress;
                }
            }
        }

        /// <summary>
        /// Deinstalls all router rules and cleans up the state
        /// </summary>
        public static void StopRouter()
        {
            foreach (RouterRule routerRule in routes)
            {
                try
                {
                    routerRule.Close();
                }
                catch { }
            }
            routes = null;
        }

        /// <summary>
        /// Receives the progress reports and passes them on
        /// </summary>
        /// <param name="retriesLeft">Number of retries left. -1 indicates a successful open. -2 indicates a failure</param>
        private static void RouterRule_MidiPortOpenProgress(int retriesLeft)
        {
            MidiInOpenProgress?.Invoke(retriesLeft);
        }
    }
}
