using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IPersonalSettingsService : IEntityService<PersonalSettings>
    {
        string GetPersonalSettingValue(long individualId, long categoryId);
        bool UpdatePersonalSettingValue(long individualId, long categoryId, string settingValue);
    }
}