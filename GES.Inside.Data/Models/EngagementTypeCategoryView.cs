using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class EngagementTypeCategoryView
    {
        public long EngagementTypeCategoriesId { get; set; }
        public string Name { get; set; }
        public System.DateTime Created { get; set; }

        public IEnumerable<EngagementTypeViewModel> EngagementTypeViewModels { get; set; }
        
    }
}