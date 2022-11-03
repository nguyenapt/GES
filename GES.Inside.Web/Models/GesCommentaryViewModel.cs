using System;
using GES.Common.Configurations;

namespace GES.Inside.Web.Models
{
    public class GesCommentaryViewModel
    {
        public long I_GesCommentary_Id { get; set; }
        public long? I_GesCaseReports_Id { get; set; }
        public string Description { get; set; }
        public DateTime? CommentaryModified { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedString => Created?.ToString(Configurations.DateWithTimeFormat);
        public string CommentaryModifiedString => CommentaryModified?.ToString(Configurations.DateWithTimeFormat);
    }
}