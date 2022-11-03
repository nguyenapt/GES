using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Services
{
    public class I_GesSourceDialogueService : EntityService<GesEntities, I_GesSourceDialogues>, II_GesSourceDialogueService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesSourceDialogueRepository _gesSourceDialogueRepository;

        public I_GesSourceDialogueService(IUnitOfWork<GesEntities> unitOfWork, II_GesSourceDialogueRepository gesSourceDialogueRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesSourceDialogueRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesSourceDialogueRepository = gesSourceDialogueRepository;
        }

        public I_GesSourceDialogues GetById(long id)
        {
            return SafeExecute(() => _gesSourceDialogueRepository.FindBy(x => x.I_GesSourceDialogues_Id == id).FirstOrDefault());
        }

        public IEnumerable<I_GesSourceDialogues> GetSourceDialogues(long gesCaseReportsId)
        {
            return SafeExecute(() => _gesSourceDialogueRepository.GetSourceDialogues(gesCaseReportsId));
        }
    }
}
