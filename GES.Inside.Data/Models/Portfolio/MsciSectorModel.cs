using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models.Portfolio
{
    public class MsciSectorModel
    {
        public long MsciId { get; set; }
        public string Sector { get; set; }
        public string IndustryGroup { get; set; }
        public string Industry { get; set; }
        public string SectorCode { get; set; }
    }
}
