using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class OldUserService : EntityService<GesEntities, G_Users>, IOldUserService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_UsersRepository _gUsersRepository;

        public OldUserService(IUnitOfWork<GesEntities> unitOfWork, IG_UsersRepository gUsersRepository, IGesLogger logger)
            : base(unitOfWork, logger, gUsersRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gUsersRepository = gUsersRepository;
        }

        public G_Users GetUserById(long userId)
        {
            return this.SafeExecute(() => _gUsersRepository.GetById(userId));
        }

        public List<G_Users> GetListOldUsers()
        {
            return _dbContext.G_Users.Include(users => users.G_Individuals1).Include(d=>d.G_Individuals1.G_Organizations).FromCache("AllOldUsers").ToList();
        }

        public List<PrimaryAndForeignKeyModel> GetIndividualIdsAndOrgIdsLastLogin(List<long> oldUserIds)
        {
            var query = from u in _dbContext.G_Users
                join i in _dbContext.G_Individuals on u.G_Individuals_Id equals i.G_Individuals_Id
                where oldUserIds.Contains(u.G_Users_Id)
                select new PrimaryAndForeignKeyModel
                {
                    Id = i.G_Individuals_Id,
                    ForeignKey = i.G_Organizations_Id
                };

            return this.SafeExecute(query.ToList);
        }

    }
}
