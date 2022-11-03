using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GES.Inside.Web.Models
{
    public class GssInternalCommentViewModel
    {
        public long Id { get; set; }

        public long GssId { get; set; }

        public string DateModified { get; set; }

        public int Level { get; set; }        

        public long User_Id { get; set; }

        public string UserName { get; set; }

        public string Comment { get; set; }
    }
}