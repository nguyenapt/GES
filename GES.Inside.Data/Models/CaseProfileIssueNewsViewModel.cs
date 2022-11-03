using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models
{
    public class CaseProfileSimpleNoteViewModel
    {
        public long NoteId { get; set; }
        [Display(Name = "Heading")]
        public string NoteTitle { get; set; }
        [Display(Name = "Body")]
        public string NoteBody { get; set; }
        [Display(Name = "Date")]
        public DateTime NoteDate { get; set; }
    }
}