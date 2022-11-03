using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IG_IndividualsRepository : IGenericRepository<G_Individuals>
    {
        G_Individuals GetById(long id);
        G_Individuals GetIndividualByUser(long userId);
    }
}
