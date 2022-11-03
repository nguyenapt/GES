using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class MsciFtseViewModel
    {
        public long Id { get; set; }
        public long? parentId { get; set; }
        public string ParentName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Lineage { get; set; }
    }
}
