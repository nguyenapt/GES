using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.DataContexts;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class PersonalSettingsRepository : GenericRepository<PersonalSettings>, IPersonalSettingsRepository
    {
        private readonly GesRefreshDbContext _dbContext;

        public PersonalSettingsRepository(GesRefreshDbContext dbContext, IGesLogger logger):base(dbContext, logger)
        {
            _dbContext = dbContext;
        }
        
        public PersonalSettings FindById(Guid personalSettingId)
        {
            return this.SafeExecute<PersonalSettings>(() => _entities.Set<PersonalSettings>().Find(personalSettingId));
        }

        public PersonalSettings GetPersonalSetting(long individualId, long categoryId)
        {
            return this.SafeExecute(() => _dbContext.PersonalSettings.FirstOrDefault(d => d.IndividualId == individualId && d.PersonalSettingCategoryId == categoryId));
        }
    }
}