using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileStatisticViewModel
    {
        [Display(Name = "Conference calls")]
        int ConferenceCount { get; set; }

        [Display(Name = "Meetings in person")]
        int MeetingCount { get; set; }

        [Display(Name = "Latest meeting")]
        string LatestMeeting { get; set; }

        [Display(Name = "Correspondence")]
        int CorrespondenceCount { get; set; }

        [Display(Name = "Number of contacts")]
        int Contacts { get; set; }
    }

    [MetadataType(typeof(ICaseProfileStatisticViewModel))]
    public class CaseProfileStatisticViewModel : ICaseProfileStatisticViewModel
    {
        public int ConferenceCount { get; set; }
        public int MeetingCount { get; set; }
        public string LatestMeeting { get; set; }
        public int CorrespondenceCount { get; set; }
        public int Contacts { get; set; }
    }
}
