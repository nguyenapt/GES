using System;

namespace GES.Inside.Data.Models
{
    public class ConventionModel
    {
        public long I_Conventions_Id { get; set; }
        public long? I_ConventionCategories_Id { get; set; }
        public string Source { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string Background { get; set; }
        public string Guidelines { get; set; }
        public string Purpose { get; set; }
        public string Administration { get; set; }
        public string GesCriteria { get; set; }
        public string GesScope { get; set; }
        public string GesRiskIndustry { get; set; }
        public string ManagementSystems { get; set; }
        public string Links { get; set; }
        public long? G_ManagedDocuments_Id { get; set; }
    }
}