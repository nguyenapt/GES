using System;

namespace GES.Inside.Data.Models
{
    public class AlertViewModel
    {
        public long I_NaArticles_Id { get; set; }
        public string Heading { get; set; }
        public string Source { get; set; }
        public DateTime? SourceDate { get; set; }
        public string Text { get; set; }
        public DateTime? AlertDate { get; set; }
    }
}
