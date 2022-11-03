using System.Collections.Generic;
using System.Linq;
using com.sun.org.apache.bcel.@internal.generic;
using GES.Common.Configurations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using ikvm.extensions;
using sun.net.www.content.image;
using System;

namespace GES.Inside.Data.Services
{
    public class GesEventCalendarUserAcceptService : EntityService<GesEntities, GesEventCalendarUserAccept>, I_GesEventCalendarUserAcceptService
    {
        private readonly GesEntities _dbContext;
        private readonly I_GesEventCalendarUserAcceptRepository _gesEventCalendarUserAcceptRepository;

        public GesEventCalendarUserAcceptService(IUnitOfWork<GesEntities> unitOfWork, I_GesEventCalendarUserAcceptRepository gesEventCalendarUserAcceptRepository, IGesLogger logger) : base(unitOfWork, logger, gesEventCalendarUserAcceptRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesEventCalendarUserAcceptRepository = gesEventCalendarUserAcceptRepository;
        }        

        public IEnumerable<GesEventCalendarUserAccept> GetAttendees(long eventId)
        {
            return SafeExecute(() => _gesEventCalendarUserAcceptRepository.GetAttendees(eventId));
        }

        public GesEventCalendarUserAccept GetByEventIdAndEmail(long eventId, string email)
        {
            return SafeExecute(() => _gesEventCalendarUserAcceptRepository.GetByEventIdAndEmail(eventId,email));
        }

        public GesEventCalendarUserAccept GetById(Guid id)
        {
            return SafeExecute(() => _gesEventCalendarUserAcceptRepository.FindBy(x => x.GesEventCalendarUserAcceptId == id).FirstOrDefault());
        }
    }
}