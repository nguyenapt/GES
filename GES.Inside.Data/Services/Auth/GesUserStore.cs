using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.DataContexts;
using Microsoft.AspNet.Identity.EntityFramework;
using GES.Common.Logging;

namespace GES.Inside.Data.Services.Auth
{
    public class GesUserStore : UserStore<GesUser, GesRole, string, GesUserLogin, GesUserRole, GesUserClaim>
    {
        public GesUserStore() : this(new GesRefreshDbContext())
        {
            base.DisposeContext = true;
        }

        public GesUserStore(GesRefreshDbContext context)
           : base(context)
        {
        }
    }
}