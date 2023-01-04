using System;
using System.Collections.Generic;

namespace MidiUWPRouter.ConfigProcessing
{
    /// <summary>
    /// Comparer object for sorting DeviceInformationEx objects
    /// </summary>
    public class DeviceComparer : IComparer<DeviceInformationEx>
    {
        /// <summary>
        /// Possible sorting fields
        /// </summary>
        public enum SortingField
        {
            Name,
            CookedName,
            Id
        }

        readonly SortingField sortField;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="field">Sets the field to use while sorting</param>
        public DeviceComparer(SortingField field)
        {
            sortField = field;
        }

        /// <summary>
        /// Case Sensitive comparing
        /// </summary>
        /// <param name="x">DeviceInformationEx x</param>
        /// <param name="y">DeviceInformationEx y</param>
        /// <returns>x less than y -> a negative value. x == y -> 0. x greater than y -> a positive value</returns>
        public int Compare(DeviceInformationEx x, DeviceInformationEx y)
        {
            switch (sortField)
            {
                case SortingField.Name:
                    return String.Compare(x.DeviceInformation.Name, y.DeviceInformation.Name, StringComparison.Ordinal);
                case SortingField.CookedName:
                    return String.Compare(x.CookedName, y.CookedName, StringComparison.Ordinal);
                case SortingField.Id:
                    return String.Compare(x.DeviceInformation.Id, y.DeviceInformation.Id, StringComparison.Ordinal);
            }
            return 0;
        }
    }
}
