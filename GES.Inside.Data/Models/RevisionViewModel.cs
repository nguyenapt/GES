using System;

namespace GES.Inside.Data.Models
{
    public class RevisionViewModel
    {
        public long I_GesCaseReportsI_GesRevisionTexts_Id { get; set; }
        public long I_GesRevisionTexts_Id { get; set; }
        public long G_GesCaseReport_Id { get; set; }
        public string GesRevisionText { get; set; }
        public bool Checked { get; set; }
        public string DateText { get; set; }
        public string Description { get; set; }
    }
}
