using System;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileStatusViewModel
    {
        long Response { get; set; }
        string ResponseName { get; set; }
        long Progress { get; set; }
        string ProgressName { get; set; }
        long Development { get; set; }
        string LatestMilestone { get; set; }
    }

    public class CaseProfileStatusViewModel : ICaseProfileStatusViewModel
    {
        public long Response { get; set; }
        public string ResponseName { get; set; }
        public long Progress { get; set; }
        public string ProgressName { get; set; }
        public long Development { get; set; }
        public string LatestMilestone { get; set; }
        public int LatestMilestoneLevel { get; set; }
    }
}
