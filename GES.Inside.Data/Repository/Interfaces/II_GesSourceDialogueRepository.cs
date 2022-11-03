using GES.Inside.Data.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesSourceDialogueRepository : IGenericRepository<I_GesSourceDialogues>
    {
        List<I_GesSourceDialogues> GetSourceDialogues(long caseProfileId);
        I_GesSourceDialogues GetById(long id);
    }
}
