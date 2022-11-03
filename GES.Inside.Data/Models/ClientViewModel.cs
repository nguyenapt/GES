using System;

namespace GES.Inside.Data.Models
{
    public class ClientViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Status { get; set; }
        public string ProgressStatus { get; set; }
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public int Employees { get; set; }
        public decimal TotalAssets { get; set; }
        public int PortfoliosNumber { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public Guid? CountryId { get; set; }
        public string CountryName { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Comment { get; set; }
    }
}
