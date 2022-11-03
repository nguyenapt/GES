using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Models
{
    public class CaseProfileDetailAdditionalDocument
    {
        
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        
    }
}