using GES.Inside.Data.Models.CaseProfiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class GesCaseProfileTemplatesViewModel
    {

        public Guid GesCaseProfileTemplatesId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDescription { get; set; }
        public long? EngagementTypeId { get; set; }
        public string EngagementType { get; set; }
        public string Recomendation { get; set; }
        public long? RecomendationId { get; set; }
        public string EntityName { get; set; }
        public string EntityCodeType { get; set; }
        public List<GesCaseProfileInvisibleEntitiesViewModel> ProfileInvisibleEntitiesViewModels { get; set; }        

    }
}