using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Common.Helpers
{
    public class RangeHelper
    {
        public IList<Range> Ranges { get; set; }
        public void Add(double from, double to, string result)
        {
            if (this.Ranges == null) Ranges = new List<Range>();

            Ranges.Add(new Range(from, to, result));
        }

        public string GetResult(double? value)
        {
            if (value.HasValue)
            {
                foreach (var item in Ranges)
                {
                    if (value >= item.From && value <= item.To)
                        return item.Result;
                }
            }
            return string.Empty;
        }
    }

    

    public class Range
    {
        public Range(double from, double to, string result)
        {
            this.From = from;
            this.To = to;
            this.Result = result;
        }
        public double From { get; set; }
        public double To { get; set; }
        public string Result { get; set; }        
    }
}
