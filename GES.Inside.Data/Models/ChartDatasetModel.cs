using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class ChartDatasetModel
    {
        public List<int> data { get; set; }
        public List<long> ids { get; set; }
        public List<string> backgroundColor { get; set; }
    }
}
