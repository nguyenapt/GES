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
    public class PersonalSettingCategoriesRepository : GenericRepository<PersonalSettingCategories>, IPersonalSettingCategoriesRepository
    {
        public PersonalSettingCategoriesRepository(GesRefreshDbContext dbContext, IGesLogger logger)
            :base(dbContext, logger)
        {
        }
        
        public PersonalSettingCategories FindById(Guid personalSettingCategoryId)
        {
            return this.SafeExecute<PersonalSettingCategories>(() => _entities.Set<PersonalSettingCategories>().Find(personalSettingCategoryId));
        }
    }
}