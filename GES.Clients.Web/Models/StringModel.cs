using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GES.Clients.Web.Models
{
    public class StringModel
    {
        public string Slug { get; set; }

        public StringModel(string slug)
        {
            Slug = slug;
        }
    }
}