using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class PrimaryAndForeignKeyModel
    {
        public long Id { get; set; }
        public long? ForeignKey { get; set; }
    }
}
