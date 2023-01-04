using MidiUWPRouter.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Windows.Devices.Enumeration;
using Windows.Devices.Enumeration.Pnp;
using Windows.Devices.Midi;

namespace MidiUWPRouter.ConfigProcessing
{
    /// <summary>
    /// Responsible for parsing a config file. The events are only informational
    /// </summary>
    internal class ConfigReader
    {
        const int maxConfigSize = 10000000;

        public delegate void RouteSkipEventHandler(string routeName, string sourceName, string destinationName);
        public static event RouteSkipEventHandler RouteSkipEvent;

        public delegate void AliasFoundEventHandler(string type, string deviceName, string aliasName);
        public static event AliasFoundEventHandler AliasFoundEvent;

        /// <summary>
        /// Gets and returns all available MidiInDevices
        /// </summary>
        /// <returns>A DeviceInformationCollection of MidiInDevices</returns>
        public static DeviceInformationCollection GetMidiInDevices()
        {
            return DeviceInformation.FindAllAsync(MidiInPort.GetDeviceSelector()).AsTask().Result;
        }

        /// <summary>
        /// Gets and returns all available MidiOutDevices
        /// </summary>
        /// <returns>A DeviceInformationCollection of MidiOutDevices</returns>
        public static DeviceInformationCollection GetMidiOutDevices()
        {
            return DeviceInformation.FindAllAsync(MidiOutPort.GetDeviceSelector()).AsTask().Result;
        }

        /// <summary>
        /// Sorts DeviceInformationCollection devInfos into a List<DeviceInformationEx> object. The 
        /// objects in this list are enriched with a field indicating the type (BLE or not) and
        /// a derived cooked name for some MIDI out devices (those with just the name "MIDI").
        /// </summary>
        /// <param name="devInfos">The collection to sort</param>
        /// <param name="isMidiIn">Indicate whether this is a list of input devices</param>
        /// <returns>The sorted list</returns>
        public static List<DeviceInformationEx> GetSortedList(DeviceInformationCollection devInfos, bool isMidiIn)
        {
            const string BusReportedDeviceDesc = "{540b947e-8b40-45bc-a8a2-6a0b894cbda2} 4";
            const string Siblings = "{4340a6c5-93fa-4706-972c-7b648008a5a7} 10";

            List<DeviceInformationEx> list = new List<DeviceInformationEx>();
            Regex hexId = new Regex(@"^SWD\\MMDEVAPI\\MIDII_([0-9A-Fa-f]{8})\..+");
            Regex midi = new Regex(@"^[0-9]* \- MIDI$|^MIDI$");
            Regex ble = new Regex(@"^SWD\\MMDEVAPI\\MIDII_[0-9A-Fa-f]{8}\.BLE.+");

            foreach (DeviceInformation devInfo in devInfos)
            {
                PnpObject pnpMidi = PnpObject.CreateFromIdAsync(PnpObjectType.Device, devInfo.Properties["System.Devices.DeviceInstanceId"].ToString(), new string[] { "System.Devices.Parent", Siblings }).AsTask().Result;

                string name = devInfo.Name;
                string[] siblingIds = pnpMidi.Properties[Siblings] as string[];
                string myId = devInfo.Properties["System.Devices.DeviceInstanceId"].ToString();

                if (siblingIds != null && midi.IsMatch(devInfo.Name) && !isMidiIn)
                {
                    // Find the input-device-sibling of this output device and use its name for this device.
                    Group m = hexId.Matches(myId)[0].Groups[1];
                    foreach (string siblingId in siblingIds)
                    {
                        if (siblingId != myId && Regex.IsMatch(siblingId, "MIDII_" + m + "\\..+"))
                        {
                            PnpObject pnpMidiSibling = PnpObject.CreateFromIdAsync(PnpObjectType.Device, siblingId, new string[] { "System.Devices.Parent", BusReportedDeviceDesc }).AsTask().Result;
                            name = pnpMidiSibling.Properties[BusReportedDeviceDesc].ToString();
                        }
                    }
                }

                list.Add(new DeviceInformationEx(devInfo, name, ble.IsMatch(myId)));
            }

            list.Sort(new DeviceComparer(DeviceComparer.SortingField.Name));
            return list;
        }

        /// <summary>
        /// Parses the {ROUTE=..] sections in the config file and creates a hashset of these RouterRules
        /// </summary>
        /// <param name="lines">The configuration to parse</param>
        /// <param name="inputs">Available input aliases</param>
        /// <param name="outputs">Available output aliases</param>
        /// <returns>A hashset with all the configured RouterRules</returns>
        /// <exception cref="MidiRouterException"></exception>
        static HashSet<RouterRule> ParseRoutings(List<string> lines, Dictionary<string, DeviceInformationEx> inputs, Dictionary<string, DeviceInformationEx> outputs)
        {
            HashSet<string> routeNames = new HashSet<string>();
            Dictionary<string, string> routePairs = new Dictionary<string, string>();
            List<string> routeLines = new List<string>();
            string routeName = "";
            int stage = 0;
            string destination = "";
            string source = "";
            HashSet<RouterRule> routings = new HashSet<RouterRule>();

            void AddRoute()
            {
                if (source == "" || destination == "")
                    throw new MidiRouterException("No source or destination configured for route {0}", routeName);
                if (routeNames.Contains(routeName))
                    throw new MidiRouterException("Route {0} already exists", routeName);
                if (routePairs.ContainsKey(source + ":" + destination))
                    throw new MidiRouterException("Duplicate routepair: {0} <-> {1}", routeName, routePairs[source + ":" + destination]);
                routePairs.Add(source + ":" + destination, routeName);
                routeNames.Add(routeName);
                if (inputs.ContainsKey(source) && outputs.ContainsKey(destination))
                    routings.Add(new RouterRule(routeName, new MidiIn(inputs[source]), new MidiOut(outputs[destination])));
                else
                    RouteSkipEvent?.Invoke(routeName, source, destination);
            }

            foreach (string line in lines)
            {
                string ln = line.Trim();
                bool repeat = true;

                while (repeat)
                {
                    repeat = false;
                    switch (stage)
                    {
                        case 0:
                            if (ln.StartsWith("[ROUTE=", StringComparison.InvariantCultureIgnoreCase) && ln.EndsWith("]"))
                            {
                                routeName = ln.Substring(7, ln.Length - 7 - 1);
                                if (routeName.Length < 3)
                                    throw new MidiRouterException("A route name (\"{0}\") must be at least 3 characters long {1}", routeName, ln);

                                source = "";
                                destination = "";
                                stage = 1;
                            }
                            break;

                        case 1:
                            if (ln.StartsWith("["))
                            {
                                AddRoute();
                                stage = 0;
                                source = "";
                                destination = "";
                                repeat = true;
                            }
                            else
                            {
                                if (ln.StartsWith("SOURCE=", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    if (source != "")
                                        throw new MidiRouterException("Source has already been defined for route {0}", routeName);
                                    source = ln.Substring(7).ToUpper();
                                }
                                else
                                {
                                    if (ln.StartsWith("DESTINATION=", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        if (destination != "")
                                            throw new MidiRouterException("Destination has already been defined for route {0}", routeName);
                                        destination = ln.Substring(12).ToUpper();
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            if (stage == 1)
                AddRoute();

            return routings;
        }

        /// <summary>
        /// Parses a configuration file inputFile. Creates a hashset of RouterRules, based on the input
        /// and output aliases configured in the router configuration.
        /// </summary>
        /// <param name="inputFile">The router configuration to parse</param>
        /// <returns>A hashset with all the configured RouterRules</returns>
        /// <exception cref="MidiRouterException"></exception>
        public static HashSet<RouterRule> ParseConfig(string inputFile)
        {
            Dictionary<string, DeviceInformationEx> inputAliases = new Dictionary<string, DeviceInformationEx>();
            Dictionary<string, DeviceInformationEx> outputAliases = new Dictionary<string, DeviceInformationEx>();

            // Read the config file in a list of strings
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(inputFile))
            {
                if (sr.BaseStream.Length < maxConfigSize)
                {
                    string ln = sr.ReadLine();
                    while (ln != null)
                    {
                        lines.Add(ln);
                        ln = sr.ReadLine();
                    }
                }
                else
                    throw new MidiRouterException("Size of config file {0} exceeds {1:n0} bytes", inputFile, maxConfigSize);
            }

            // Parse the [MIDIIN=..] alias definitions and the [MIDIOUT=..] alias definitions
            AliasFilterSets inAliasFilterSets = new AliasFilterSets(lines, true);
            AliasFilterSets outAliasFilterSets = new AliasFilterSets(lines, false);

            // Get sorted device-lists for in- and outputs. DeviceInformationEx objects are used, because they
            // are somewhat enriched with extra useful data.
            List<DeviceInformationEx> sortedOutputDevices = GetSortedList(GetMidiOutDevices(), false);
            List<DeviceInformationEx> sortedInputDevices = GetSortedList(GetMidiInDevices(), true);

            // Associate the inputdevices with aliases
            foreach (DeviceInformationEx devInfo in sortedInputDevices)
            {
                string alias = inAliasFilterSets.Search(devInfo);
                if (alias != null)
                {
                    inputAliases.Add(alias, devInfo);
                    AliasFoundEvent?.Invoke("In", devInfo.CookedName, alias);
                }
            }

            // Associate the outputdevices with aliases
            foreach (DeviceInformationEx devInfo in sortedOutputDevices)
            {
                string alias = outAliasFilterSets.Search(devInfo);
                if (alias != null)
                {
                    outputAliases.Add(alias, devInfo);
                    AliasFoundEvent?.Invoke("Out", devInfo.CookedName, alias);
                }
            }

            // Parse the routings and create and HashSet of inactive RouterRules and return it.
            return ParseRoutings(lines, inputAliases, outputAliases);
        }
    }
}
