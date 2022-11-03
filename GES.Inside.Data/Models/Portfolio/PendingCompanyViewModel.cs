using System;

namespace GES.Inside.Data.Models.Portfolio
{
    public class PendingCompanyViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Isin { get; set; }
        public string Sedol { get; set; }
        public long? MasterCompanyId { get; set; }

        public bool Screened { get; set; }
        public bool SelectedToAdd { get; set; }
        public bool SelectedToDelete { get; set; }
    }
}
