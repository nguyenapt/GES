using GES.Inside.Data.Models.CaseProfiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class GesCaseProfileInvisibleEntitiesViewModel
    {
        public Guid GesCaseProfileInvisibleEntity_Id { get; set; }
        public Guid GesCaseProfileTemplates_Id { get; set; }
        public Guid GesCaseProfileEntity_Id { get; set; }
        public string EntityName { get; set; }
        public string EntityCodeType { get; set; }
        public int? InVisibleType { get; set; }
    }
}