using System;

namespace GES.Inside.Data.Models
{
    public class CaseProfileDetailAdditionalIncident
    {
        public long CaseProfileId { get; set; }
        public string Issue  { get; set; }
        public DateTime? AlertDate  { get; set; }
        public string Location  { get; set; }
        public string HomeCountry  { get; set; }
        public string EngagementThemeNorm  { get; set; }
        public string Recommentdation  { get; set; }
        public bool Confirmed  { get; set; }
        public string Endorse  { get; set; }
    }
}