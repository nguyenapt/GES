using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesAssociatedCorporationsRepository : IGenericRepository<I_GesAssociatedCorporations>
    {
        IList<AssociatedCorporationsViewModel> GetAssociatedCorporationsByCaseProfile(long caseprofileId);

        I_GesAssociatedCorporations GetById(long id);
    }
}
