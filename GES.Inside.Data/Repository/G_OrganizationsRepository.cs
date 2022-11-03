using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Models;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository
{
    public class G_OrganizationsRepository : GenericRepository<G_Organizations>, IG_OrganizationsRepository
    {
        private readonly GesEntities _dbContext;

        public G_OrganizationsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public IEnumerable<ClientViewModel> GetAllClients()
        {
            return from o in _dbContext.G_Organizations
                               join os in _dbContext.G_OrganizationsI_ClientStatuses on o.G_OrganizationsI_ClientStatuses_Id equals os.G_OrganizationsI_ClientStatuses_Id into ps
                               from os in ps.DefaultIfEmpty()
                               join cs in _dbContext.I_ClientStatuses on os.I_ClientStatuses_Id equals cs.I_ClientStatuses_Id into ops
                               from cs in ops.DefaultIfEmpty()
                               join cps in _dbContext.I_ClientProgressStatuses on o.I_ClientProgressStatuses_Id equals cps.I_ClientProgressStatuses_Id into ocps
                               from cps in ocps.DefaultIfEmpty()
                               join i in _dbContext.G_Industries on o.G_Industries_Id equals i.G_Industries_Id into ips
                               from i in ips.DefaultIfEmpty()
                               join c in _dbContext.Countries on o.CountryId equals c.Id into co
                               from c in co.DefaultIfEmpty()

                               where o.Customer == true
                               select new ClientViewModel()
                               {
                                   Id = o.G_Organizations_Id,
                                   Name = o.Name.Trim(),
                                   Industry = i == null ? null : i.Name,
                                   Status = cs == null ? null : cs.Name,
                                   ProgressStatus = cps == null ? null : cps.Name,
                                   Country = c == null ? null : c.Name,
                                   Created = o.Created,
                                   Modified = o.Modified,
                                   Employees = o.Employees ?? 0,
                                   TotalAssets = o.TotalAssets ?? 0,
                                   PortfoliosNumber = o.I_PortfoliosG_Organizations.Count()
                               };
        }

        public IEnumerable<ClientViewModel> GetAllOrganizations()
        {
            return from o in _dbContext.G_Organizations
                   join os in _dbContext.G_OrganizationsI_ClientStatuses on o.G_OrganizationsI_ClientStatuses_Id equals os.G_OrganizationsI_ClientStatuses_Id into ps
                   from os in ps.DefaultIfEmpty()
                   join cs in _dbContext.I_ClientStatuses on os.I_ClientStatuses_Id equals cs.I_ClientStatuses_Id into ops
                   from cs in ops.DefaultIfEmpty()
                   join cps in _dbContext.I_ClientProgressStatuses on o.I_ClientProgressStatuses_Id equals cps.I_ClientProgressStatuses_Id into ocps
                   from cps in ocps.DefaultIfEmpty()
                   join i in _dbContext.G_Industries on o.G_Industries_Id equals i.G_Industries_Id into ips
                   from i in ips.DefaultIfEmpty()
                   join c in _dbContext.Countries on o.CountryId equals c.Id into co
                   from c in co.DefaultIfEmpty()
                   
                   select new ClientViewModel()
                   {
                       Id = o.G_Organizations_Id,
                       Name = o.Name.Trim(),
                       Industry = i == null ? null : i.Name,
                       Status = cs == null ? null : cs.Name,
                       ProgressStatus = cps == null ? null : cps.Name,
                       Country = c == null ? null : c.Name,
                       Created = o.Created,
                       Modified = o.Modified,
                       Employees = o.Employees ?? 0,
                       TotalAssets = o.TotalAssets ?? 0,
                       PortfoliosNumber = o.I_PortfoliosG_Organizations.Count()
                   };
        }

        public G_Organizations GetById(long id)
        {
            return this.SafeExecute<G_Organizations>(() => _entities.Set<G_Organizations>().FirstOrDefault(d => d.G_Organizations_Id == id));
        }
    }
}
