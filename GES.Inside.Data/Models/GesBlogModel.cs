using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GES.Inside.Data.Models
{
    public class GesBlogModel
    {        
        public string Title { get; set; }
        public string LinkTitle { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
}
