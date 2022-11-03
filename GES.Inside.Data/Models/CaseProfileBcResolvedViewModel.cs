using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public class CaseProfileBcResolvedViewModel : CaseProfileBcDisengageViewModel
    {
        public ICaseProfileEngagementInformationViewModel EngagementInformationComponent { get; set; }
    }
}