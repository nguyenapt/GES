using System;

namespace GES.Inside.Data.Models
{
    public class GesUngpAssessmentScoresViewModel
    {
        public Guid GesUngpAssessmentScoresId { get; set; }
        public string Name { get; set; }
        public string ScoreType { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public double Score { get; set; }
        public DateTime? Created { get; set; }
    }
}