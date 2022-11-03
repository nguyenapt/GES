using System;

namespace GES.Inside.Data.Models
{
    public class NormModel
    {
        public long I_Norms_Id { get; set; }
        public long? I_NormCategories_Id { get; set; }
        public string GesCode { get; set; }
        public string Code { get; set; }
        public string Source { get; set; }
        public string SourceShort { get; set; }
        public string SiriNormArea { get; set; }
        public string SectionTitle { get; set; }
        public string SectionText { get; set; }
        public string SectionNr { get; set; }
        public string SectionArea { get; set; }
        public string FriendlyName { get; set; }
        public string PNr { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string FullText { get; set; }
        public bool Active { get; set; }
        public bool Sweden { get; set; }
        public int? ExternalId { get; set; }
    }
}