using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.Indicators.Lines
{
    /// <summary>
    /// Represents an interception between two Pointlines.
    /// </summary>
    public class InterceptionInfo
    {
        /// <summary>
        /// Start index (just before interception).
        /// </summary>
        public int StartIndex { get; }
        /// <summary>
        /// End index (just after interception). It's always equal to StartIndex +1.
        /// </summary>
        public int EndIndex { get; }

        /// <summary>
        /// Value of first line in StartIndex.
        /// </summary>
        public decimal StartValue1 { get; }
        /// <summary>
        /// Value of second line in StartIndex.
        /// </summary>
        public decimal StatrValue2 { get; }
        /// <summary>
        /// Value of first line in EndIndex.
        /// </summary>
        public decimal EndValue1 { get; }
        /// <summary>
        /// Value of second line in EndIndex.
        /// </summary>
        public decimal EndValue2 { get; }

        /// <summary>
        /// Initializes a new instance of InterceptionInfo object.
        /// </summary>
        /// <param name="startIndex">Start index (just before interception)</param>
        /// <param name="start1">Value of first line in StartIndex.</param>
        /// <param name="end1">Value of first line in EndIndex.</param>
        /// <param name="start2">Value of second line in StartIndex.</param>
        /// <param name="end2">Value of second line in EndIndex.</param>
        public InterceptionInfo(int startIndex, decimal start1, decimal end1, decimal start2, decimal end2)
        {
            StartIndex = startIndex;
            EndIndex = startIndex + 1;
            StartValue1 = start1;
            EndValue1 = end1;
            StatrValue2 = start2;
            EndValue2 = end2;
        }
    }
}
