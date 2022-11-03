using System;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IPersonalSettingCategoriesRepository : IGenericRepository<PersonalSettingCategories>
    {
        PersonalSettingCategories FindById(Guid pesonalSettingCategoryId);
    }
}