using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class IdNameModelPresetsExtendedModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<BasicControActivPresetItemModel> Items { get; set; }
    }
}
