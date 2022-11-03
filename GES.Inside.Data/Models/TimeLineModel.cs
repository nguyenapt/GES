using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class TimeLineModel
    {
        public DateTime DateTime { get; set; }
        public TimeLinePointModel PointProperty { get; set; }
    }
}
