using GES.Inside.Data.Models.CaseProfiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class GesCaseProfileEntitiesGroupViewModel
    {
        public Guid GesCaseProfileEntitiesGroupId { get; set; }
        public string Name { get; set; }
        public string GroupType { get; set; }
        public int? Order { get; set; }
        public IEnumerable<GesCaseProfileEntitiesViewModel> CaseProfileEntities { get; set; }
    }
}