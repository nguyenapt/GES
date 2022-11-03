using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class I_NormsService: EntityService<GesEntities, I_Norms>, II_NormsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_NormsRepository _normsRepository;

        public I_NormsService(IUnitOfWork<GesEntities> unitOfWork, II_NormsRepository normsRepository, IGesLogger logger)
            : base(unitOfWork, logger, normsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _normsRepository = normsRepository;
        }

        public I_Norms GetById(long id)
        {
            return this.SafeExecute<I_Norms>(() => _normsRepository.FindBy(x => x.I_Norms_Id == id).FirstOrDefault());
        }

        public IEnumerable<NormModel> GetAllNorms()
        {
            var query = from m in _dbContext.I_Norms
                where m.FriendlyName != null
                select new NormModel()
                {
                    I_Norms_Id  = m.I_Norms_Id ,
                    I_NormCategories_Id  = m.I_NormCategories_Id ,
                    GesCode  = m.GesCode ,
                    Code  = m.Code ,
                    Source  = m.Source ,
                    SourceShort  = m.SourceShort ,
                    SiriNormArea  = m.SiriNormArea ,
                    SectionTitle  = m.SectionTitle ,
                    SectionText  = m.SectionText ,
                    SectionNr  = m.SectionNr ,
                    SectionArea  = m.SectionArea ,
                    FriendlyName  = m.FriendlyName ,
                    PNr  = m.PNr ,
                    Text  = m.Text ,
                    Url  = m.Url ,
                    FullText  = m.FullText ,
                    Active  = m.Active ,
                    Sweden  = m.Sweden ,
                    ExternalId  = m.ExternalId 
                };

            return query.OrderBy(d => d.SectionTitle).ToList();

        }

        public PaginatedResults<NormModel> GetAllConventionsForGrid(JqGridViewModel jqGridParams)
        {
            var query = GetAllNorms();

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "name":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.FriendlyName)
                            : query.OrderByDescending(x => x.FriendlyName);
                        break;
                    case "sectiontitle":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.SectionTitle)
                            : query.OrderByDescending(x => x.SectionTitle);
                        break;
                    default:
                        break;
                }
            }

            if (!jqGridParams._search) return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
            var finalRules = JqGridHelper.GetFilterRules<NormModel>(jqGridParams);
            query = string.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }
    }
}
