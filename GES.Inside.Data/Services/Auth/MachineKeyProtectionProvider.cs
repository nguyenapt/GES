﻿using Microsoft.Owin.Security.DataProtection;

namespace GES.Inside.Data.Services.Auth
{
    public class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }
}
