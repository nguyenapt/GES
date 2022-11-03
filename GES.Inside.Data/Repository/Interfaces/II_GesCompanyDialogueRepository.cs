using GES.Inside.Data.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCompanyDialogueRepository: IGenericRepository<I_GesCompanyDialogues>
    {
        List<I_GesCompanyDialogues> GetCompanyDialogues(long caseProfileId);

        I_GesCompanyDialogues GetById(long id);
    }
}
