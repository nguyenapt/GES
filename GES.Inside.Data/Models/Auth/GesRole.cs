using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GES.Inside.Data.Models.Auth
{
    public class GesRole : IdentityRole<string, GesUserRole>
    {
        public GesRole() : base()
        {
            
        }

        // extra properties here
    }
}