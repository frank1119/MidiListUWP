using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MidiUWPRouter.ConfigProcessing
{
    /// <summary>
    /// In the config filters are specified for a particular alias. The first device that matches
    /// one of these filters of a particular alias should be associated with that alias.
    /// After processing all devices each alias is associated with 0 or 1 devices.
    /// Physical devices might have different names when inserted in different USB ports, so by 
    /// having multiple filters for an alias the physical device can be mapped to the same alias
    /// although the name (slightly) differs.
    /// </summary>
    public class AliasFilterSets
    {
        readonly List<AliasFilterSet> aliasFilterSet = new List<AliasFilterSet>();
        

        /// <summary>
        /// A set of filters for a particular alias
        /// </summary>
        public class AliasFilterSet
        {
            public readonly string Alias;
            private readonly List<AliasFilter> aliasFilters = new List<AliasFilter>();

            /// <summary>
            /// An alias filter rule 
            /// </summary>
            public class AliasFilter
            {
                enum FilterType
                {
                    ID,
                    Regex,
                    Name,
                    RealName,
                    CookedRegex
                }

                private readonly FilterType filterType;
                private readonly Regex reg;
                private readonly string arg;

                /// <summary>
                /// Parses a configuration line to derive a filter from it.
                /// </summary>
                /// <param name="line">Line with format '[ID|NAME|REGEX|REALNAME|REALREGEX]:[arg]</param>
                /// <exception cref="MidiRouterException"></exception>
                public AliasFilter(string line)
                {
                    line = line.Trim();

                    string[] ft = line.Split(new[] { ':' }, StringSplitOptions.None);
                    if (ft.Length < 1)
                    {
                        throw new MidiRouterException("Invalid filter rule: {0}", line);
                    }

                    switch (ft[0].ToUpperInvariant())
                    {
                        case "ID":
                            filterType = FilterType.ID;
                            arg = line.Substring(3);
                            break;
                        case "NAME":
                            filterType = FilterType.Name;
                            arg = line.Substring(5);
                            break;
                        case "REALNAME":
                            filterType = FilterType.RealName;
                            arg = line.Substring(9);
                            break;
                        case "REGEX":
                            filterType = FilterType.Regex;
                            arg = line.Substring(6);
                            reg = new Regex(arg);
                            break;
                        case "REALREGEX":
                            filterType = FilterType.CookedRegex;
                            arg = line.Substring(10);
                            reg = new Regex(arg);
                            break;
                        default:
                            throw new MidiRouterException("Invalid filter rule: {0}", line);
                    }
                }

                /// <summary>
                /// Matches an attribute of devInfo against this filter
                /// </summary>
                /// <param name="devInfo">The DeviceInformationEx to test</param>
                /// <returns>True if the device matches</returns>
                internal bool Match(DeviceInformationEx devInfo)
                {
                    switch (filterType)
                    {
                        case FilterType.ID:
                            return devInfo.DeviceInformation.Properties["System.Devices.DeviceInstanceId"].ToString() == arg;
                        case FilterType.Regex:
                            return reg.IsMatch(devInfo.CookedName);
                        case FilterType.Name:
                            return devInfo.CookedName == arg;
                        case FilterType.CookedRegex:
                            return reg.IsMatch(devInfo.DeviceInformation.Name);
                        case FilterType.RealName:
                            return devInfo.DeviceInformation.Name == arg;
                    }
                    return false;
                }
            }


            /// <summary>
            /// Tests the device devInfo against all filters until one matches
            /// </summary>
            /// <param name="devInfo">The device to match with the rules</param>
            /// <returns>Returns true if a match is found</returns>
            public bool Match(DeviceInformationEx devInfo)
            {
                foreach (AliasFilter f in aliasFilters)
                {
                    if (f.Match(devInfo))
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Constructs a complete filterset from lines
            /// </summary>
            /// <param name="lines">Lines with format '[ID|NAME|REGEX|REALNAME|REALREGEX]:[arg]</param>
            /// <param name="alias">The alias associated with these filters</param>
            public AliasFilterSet(List<string> lines, string alias)
            {
                foreach (string line in lines)
                {
                    aliasFilters.Add(new AliasFilter(line));
                }
                Alias = alias;
            }
        }

        /// <summary>
        /// Constructs all AliasFilterSets from the configuration file
        /// </summary>
        /// <param name="inFile">List of strings with the configuration</param>
        /// <param name="inputs">Set to True when creating input device filters, set to False for output device filters</param>
        /// <exception cref="MidiRouterException"></exception>
        public AliasFilterSets(List<string> inFile, bool inputs)
        {
            HashSet<string> aliases = new HashSet<string>();
            string startLine = inputs ? "[MIDIIN=" : "[MIDIOUT=";
            List<string> filterLines = new List<string>();
            int stage = 0;
            string alias = "";

            // Local function 
            void AddFilterSet()
            {
                if (filterLines.Count == 0)
                    throw new MidiRouterException("Empty Alias {0} Filter Set", alias);
                if (aliases.Contains(alias))
                    throw new MidiRouterException("Alias {0} already exists", alias);
                aliases.Add(alias);
                AliasFilterSet afs = new AliasFilterSet(filterLines, alias);
                aliasFilterSet.Add(afs);
            }

            foreach (string line in inFile)
            {
                string ln = line.Trim();
                bool repeat = true;

                while (repeat)
                {
                    repeat = false;
                    switch (stage)
                    {
                        case 0:
                            if (ln.StartsWith(startLine, StringComparison.InvariantCultureIgnoreCase) && ln.EndsWith("]"))
                            {
                                filterLines = new List<string>();
                                alias = ln.Substring(startLine.Length, ln.Length - 1 - startLine.Length);
                                if (alias.Length < 3)
                                    throw new MidiRouterException("An alias name (\"{0}\") must be at least 3 characters long {1}", alias, ln);
                                if (alias.Contains(":"))
                                    throw new MidiRouterException("An alias name (\"{0}\") cannot contain a colon (:) {1}", alias, ln);

                                stage = 1;
                            }
                            break;

                        case 1:
                            if (ln.StartsWith("["))
                            {
                                AddFilterSet();
                                filterLines = new List<string>();
                                stage = 0;
                                repeat = true;
                            }
                            else
                            {
                                if (ln.StartsWith("FILTER=", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    filterLines.Add(ln.Substring(7));
                                }
                            }
                            break;
                    }
                }
            }
            if (stage == 1)
                AddFilterSet();

        }

        /// <summary>
        /// Searches for an alias with a filter that matches the name, cooked name or id
        /// of the devInfo. If a matching alias is found, the alias-filter ruleset is removed (to
        /// prevent associating an other device getting the same alias) and the alias name is returned.
        /// </summary>
        /// <param name="devInfo">The device to find an alias for</param>
        /// <returns>The found alias name, or null when the device doesn't match any filter of any alias</returns>
        public string Search(DeviceInformationEx devInfo)
        {
            AliasFilterSet foundAfs = null;
            foreach (AliasFilterSet afs in aliasFilterSet)
            {
                if (afs.Match(devInfo))
                {
                    foundAfs = afs;
                    break;
                }
            }

            if (foundAfs != null)
            {
                aliasFilterSet.Remove(foundAfs);
                return foundAfs.Alias.ToUpper();
            }
            return null;
        }
    }
}
