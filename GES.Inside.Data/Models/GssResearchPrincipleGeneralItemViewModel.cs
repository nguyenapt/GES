using System;

namespace GES.Inside.Data.Models
{
    public class GssResearchPrincipleGeneralItemViewModel
    {
        public Guid Id { get; set; }
        public Guid GssId { get; set; }
        public string Type { get; set; }
        public DateTime DateModified { get; set; }
        public string Description { get; set; }
    }
}
