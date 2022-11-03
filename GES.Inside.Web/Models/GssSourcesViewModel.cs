using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GES.Inside.Web.Models
{
    public class GssSourcesViewModel
    {
        public Guid Id { get; set; }

        public Guid GssId { get; set; }

        public DateTime? DateModified { get; set; }

        public string Description { get; set; }
    }
}