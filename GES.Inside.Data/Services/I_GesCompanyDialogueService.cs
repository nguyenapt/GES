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
    public class I_GesCompanyDialogueService : EntityService<GesEntities, I_GesCompanyDialogues>, II_GesCompanyDialogueService
    {
        private readonly GesEntities _dbContext;
        private readonly II_GesCompanyDialogueRepository _gesCompanyDialogueRepository;

        public I_GesCompanyDialogueService(IUnitOfWork<GesEntities> unitOfWork, II_GesCompanyDialogueRepository gesCompanyDialogueRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesCompanyDialogueRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesCompanyDialogueRepository = gesCompanyDialogueRepository;
        }

        public I_GesCompanyDialogues GetById(long id)
        {
            return SafeExecute(() => _gesCompanyDialogueRepository.FindBy(x=>x.I_GesCompanyDialogues_Id==id).FirstOrDefault());
        }

        public IEnumerable<I_GesCompanyDialogues> GetCompanyDialogues(long gesCaseReportsId)
        {
            return SafeExecute(() => _gesCompanyDialogueRepository.GetCompanyDialogues(gesCaseReportsId));            
        }
    }
}
