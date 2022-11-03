using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.DataContexts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GES.Inside.Data.Services.Auth
{
    public class GesRoleStore : RoleStore<GesRole, string, GesUserRole>
    {
        public GesRoleStore(GesRefreshDbContext context) : base(context)
        {

        }
    }
}