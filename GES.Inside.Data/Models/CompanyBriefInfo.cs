namespace GES.Inside.Data.Models
{
    public class CompanyBriefInfo
    {
        public string Name { get; set; }
        public long? MasterCompanyId { get; set; }
        public string MasterCompanyName { get; set; }
        public long CompanyId { get; set; }
        public string Isin { get; set; }
    }
}