using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models.Portfolio
{
    public class PortfolioControActivPresetListViewModel
    {
        public long ControActivPresetId { get; set; }
        public string ControActivPresetName { get; set; }
        public Int32 ControversialActivities { get; set; }
        public DateTime Created { get; set; }
    }
}
