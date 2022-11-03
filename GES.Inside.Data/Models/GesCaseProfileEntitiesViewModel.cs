using GES.Inside.Data.Models.CaseProfiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class GesCaseProfileEntitiesViewModel
    {

        public string id { get; set; }
        public string name { get; set; }
        public int? Order { get; set; }
        public Guid GesCaseProfileEntity_Id { get; set; }
    }
}