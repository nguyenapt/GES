using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_CompaniesI_ManagementSystemsRepository : IGenericRepository<I_CompaniesI_ManagementSystems>
    {
        I_CompaniesI_ManagementSystems GetById(long id);
    }
}
