using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IG_IndividualsService : IEntityService<G_Individuals>
    {
        G_Individuals GetIndividualByUserId(long userId);
        G_Individuals GetById(long gIndividualId);
        bool CheckServiceForIndividual(long individualId);
        IList<DialogueCaseModel> GetCompanyDialogueLogsByIndividual(long individualId);
        IList<DialogueCaseModel> GetSourceDialogueLogsByIndividual(long individualId);
    }
}
