using System;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class PersonalSettingsService : EntityService<GesRefreshDbContext, PersonalSettings>, IPersonalSettingsService
    {
        private readonly GesRefreshDbContext _dbContext;
        private readonly IPersonalSettingsRepository _personalSettingRepository;

        public PersonalSettingsService(IUnitOfWork<GesRefreshDbContext> unitOfWork, IPersonalSettingsRepository personalSettingRepository, IGesLogger logger)
            : base(unitOfWork, logger, personalSettingRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _personalSettingRepository = personalSettingRepository;
        }

        public string GetPersonalSettingValue(long individualId, long categoryId)
        {
            var setting = this.SafeExecute(() => this._personalSettingRepository.GetPersonalSetting(individualId, categoryId));
            return setting?.SettingValue ?? string.Empty;
        }
        
        public bool UpdatePersonalSettingValue(long individualId, long categoryId, string settingValue)
        {
            var setting = this.SafeExecute(() => this._personalSettingRepository.GetPersonalSetting(individualId, categoryId));
            if (setting != null)
            {
                setting.SettingValue = settingValue;
                setting.ModifiedDate = DateTime.UtcNow;
                Update(setting);
            }
            else
            {
                setting = new PersonalSettings
                {
                    IndividualId = individualId,
                    PersonalSettingCategoryId = categoryId,
                    SettingValue = settingValue,
                    CreatedDate = DateTime.UtcNow
                };
                Add(setting);
            }

            this.UnitOfWork.Commit();

            return true;
        }
    }
}