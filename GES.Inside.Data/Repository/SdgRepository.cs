using System;
using System.Collections.Generic;
using System.Linq;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class SdgRepository : GenericRepository<Sdg>, ISdgRepository
    {
        private readonly GesEntities _dbContext;

        public SdgRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public IEnumerable<Sdg> GetSdgs()
        {
            return SafeExecute(() => _dbContext.Sdg);
        }

        public IEnumerable<Sdg> GetSdgsByOrder(IList<long> sdgIds)
        {
            var sdgs = new List<Sdg>();
            foreach (var sdgId in sdgIds)
            {
                sdgs.Add(_dbContext.Sdg.FirstOrDefault(x => x.Sdg_Id == sdgId));
            }
            return sdgs;
        }

        public Sdg GetById(long id)
        {
            return SafeExecute(() => _dbContext.Sdg.FirstOrDefault(x => x.Sdg_Id == id));
        }

        public bool TryDeleteSdg(long sdgId)
        {
            try
            {
                var sdg = GetById(sdgId);
                Delete(sdg);
                Save();
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to delete SDG");
                return false;
            }
        }
    }
}
