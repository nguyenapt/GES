using System;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IPersonalSettingsRepository : IGenericRepository<PersonalSettings>
    {
        PersonalSettings FindById(Guid pesonalSettingId);
        PersonalSettings GetPersonalSetting(long individualId, long categoryId);
    }
}