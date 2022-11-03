using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Models
{
    public class CaseProfileDetailCalendar
    {
        
        public long CaseProfileId { get; set; }
        public DateTime? Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
    }
}