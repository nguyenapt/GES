using System;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public class PublishedComponent : CaseProfileComponent
    {
        public DateTime PublishedDate { get; set; }
        public string Content { get; set; }

        public long? Id { get; set; }
    }
}
