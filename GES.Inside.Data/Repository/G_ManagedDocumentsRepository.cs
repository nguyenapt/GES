using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using AutoMapper;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class G_ManagedDocumentsRepository : GenericRepository<G_ManagedDocuments>, IG_ManagedDocumentsRepository
    {
        private GesEntities _dbContext;
        public G_ManagedDocumentsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public G_ManagedDocuments GetById(long id)
        {
            return this.SafeExecute<G_ManagedDocuments>(() => _entities.Set<G_ManagedDocuments>().FirstOrDefault(d => d.G_ManagedDocuments_Id == id));
        }

        public IEnumerable<DocumentViewModel> GetAdditionalDocuments(long caseProfileId)
        {
            var result = this.SafeExecute<IEnumerable<DocumentViewModel>>(() => from gcr in _dbContext.I_GesCaseReports
                                                                          join gc in _dbContext.I_GesCompanies on gcr.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                                                          join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                                                                          join cmd in _dbContext.I_CompaniesG_ManagedDocuments on c.I_Companies_Id equals cmd.I_Companies_Id
                                                                          join d in _dbContext.G_ManagedDocuments on cmd.G_ManagedDocuments_Id equals d.G_ManagedDocuments_Id
                                                                          join u in _dbContext.G_Uploads on d.G_ManagedDocuments_Id equals (int)(u.G_ManagedDocuments_Id ?? default(int)) into ug
                                                                          from u in ug.DefaultIfEmpty()
                                                                          where gcr.I_GesCaseReports_Id == caseProfileId && d.G_ManagedDocumentServices_Id == (int)DocumentService.ExternalDocument
                                                                                orderby u.Created descending
                                                                          select new DocumentViewModel
                                                                          {
                                                                              Name = d.Name,
                                                                              FileName = u.FileName,
                                                                              I_Companies_Id = cmd.I_Companies_Id,
                                                                              I_GesCaseReports_Id = gcr.I_GesCaseReports_Id,
                                                                              G_ManagedDocuments_Id = d.G_ManagedDocuments_Id,
                                                                              Comment = d.Comment,
                                                                              Created = d.Created
                                                                          }).Union(from gcr in _dbContext.I_GesCaseReports
                                                                            join gc in _dbContext.I_GesCompanies on gcr.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                                                                            join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                                                                            join cmd in _dbContext.I_GesCaseReportsG_ManagedDocuments on gcr.I_GesCaseReports_Id equals cmd.I_GesCaseReports_Id
                                                                            join d in _dbContext.G_ManagedDocuments on cmd.G_ManagedDocuments_Id equals d.G_ManagedDocuments_Id
                                                                            join u in _dbContext.G_Uploads on d.G_ManagedDocuments_Id equals (int)(u.G_ManagedDocuments_Id ?? default(int)) into ug
                                                                            from u in ug.DefaultIfEmpty()
                                                                            where gcr.I_GesCaseReports_Id == caseProfileId && d.G_ManagedDocumentServices_Id == (int)DocumentService.CaseProfile
                                                                                   orderby u.Created descending
                                                                            select new DocumentViewModel
                                                                            {
                                                                                Name = d.Name,
                                                                                FileName = u.FileName,
                                                                                I_GesCaseReports_Id = gcr.I_GesCaseReports_Id,
                                                                                I_Companies_Id = gc.I_Companies_Id,
                                                                                G_ManagedDocuments_Id = d.G_ManagedDocuments_Id,
                                                                                Comment = d.Comment,
                                                                                Created = d.Created
                                                                            });

            return result;
        }

        public IEnumerable<DocumentViewModel> GetUploadedDocuments(long companyId)
        {
            return from md in _dbContext.G_ManagedDocuments
                   join cm in _dbContext.I_CompaniesG_ManagedDocuments on md.G_ManagedDocuments_Id equals
                       cm.G_ManagedDocuments_Id ?? -1
                   join u in _dbContext.G_Uploads on md.G_ManagedDocuments_Id equals u.G_ManagedDocuments_Id ?? -1 into gu
                   from u in gu.DefaultIfEmpty()
                   where cm.I_Companies_Id == companyId && md.G_ManagedDocumentServices_Id == (int)DocumentService.ExternalDocument
                   select new DocumentViewModel
                   {
                       I_Companies_Id = cm.I_Companies_Id,
                       I_GesCaseReports_Id = 0,
                       G_ManagedDocuments_Id = md.G_ManagedDocuments_Id,
                       Name = md.Name,
                       Comment = md.Comment,
                       FileName = u.FileName,
                       Created = md.Created
                   };
        }

        public DocumentViewModel GetUploadedDocumentById(long documentId)
        {
            var document = GetById(documentId);

            if (document == null) return null;

            var documentView = Mapper.Map<DocumentViewModel>(document);
            var upload = _dbContext.G_Uploads.FirstOrDefault(x => x.G_ManagedDocuments_Id == documentId);
            documentView.FileName = upload?.FileName;
            return documentView;
        }

        public DocumentViewModel GetDocumentByCompanySourceDialogId(long companySourceDialogId, string dialogType)
        {
            if (dialogType == "Company")
            {
                return (from md in _dbContext.G_ManagedDocuments
                    join cm in _dbContext.I_GesCompanyDialogues on md.G_ManagedDocuments_Id equals
                    cm.G_ManagedDocuments_Id
                    join u in _dbContext.G_Uploads on md.G_ManagedDocuments_Id equals u.G_ManagedDocuments_Id ?? -1 into
                    gu
                    from u in gu.DefaultIfEmpty()
                    where cm.I_GesCompanyDialogues_Id == companySourceDialogId
                    select new DocumentViewModel
                    {
                        G_ManagedDocuments_Id = md.G_ManagedDocuments_Id,
                        Name = md.Name,
                        FileName = u.FileName,
                        Created = md.Created
                    }).FirstOrDefault();
            }
            else
            {
                return (from md in _dbContext.G_ManagedDocuments
                    join cm in _dbContext.I_GesSourceDialogues on md.G_ManagedDocuments_Id equals
                    cm.G_ManagedDocuments_Id
                    join u in _dbContext.G_Uploads on md.G_ManagedDocuments_Id equals u.G_ManagedDocuments_Id ?? -1 into
                    gu
                    from u in gu.DefaultIfEmpty()
                    where cm.I_GesSourceDialogues_Id == companySourceDialogId
                    select new DocumentViewModel
                    {
                        G_ManagedDocuments_Id = md.G_ManagedDocuments_Id,
                        Name = md.Name,
                        FileName = u.FileName,
                        Created = md.Created
                    }).FirstOrDefault();
            }
        }
        
        public IEnumerable<DocumentViewModel> GetDocumentViewModel()
        {
           
            return this.SafeExecute<IEnumerable<DocumentViewModel>>(() => from  d in _dbContext.G_ManagedDocuments 
                
                join cmd in _dbContext.I_CompaniesG_ManagedDocuments on d.G_ManagedDocuments_Id equals cmd.G_ManagedDocuments_Id into cmds
                from cmd in cmds.DefaultIfEmpty()
                join u in _dbContext.G_Uploads on d.G_ManagedDocuments_Id equals (int)(u.G_ManagedDocuments_Id ?? default(int)) into ug
                from u in ug.DefaultIfEmpty()
                join s in _dbContext.G_ManagedDocumentServices on d.G_ManagedDocumentServices_Id equals s.G_ManagedDocumentServices_Id into sv
                from s in sv.DefaultIfEmpty()
                join c in _dbContext.I_Companies on cmd.I_Companies_Id equals c.I_Companies_Id into co
                from c in co.DefaultIfEmpty()
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id into gcs
                from gc in gcs.DefaultIfEmpty()
                join gcr in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals gcr.I_GesCompanies_Id into gcrs
                from gcr in gcrs.DefaultIfEmpty() 
                where d.G_ManagedDocumentServices_Id == (long)GManagedDocumentService.Company || d.G_ManagedDocumentServices_Id == (long)GManagedDocumentService.CaseProfile
                orderby u.Created descending
                select new DocumentViewModel
                {
                    G_ManagedDocuments_Id = d.G_ManagedDocuments_Id,
                    Name = d.Name,
                    CompanyName = c.MsciName,
                    I_Companies_Id = c.I_Companies_Id,
                    ReportIncident = gcr.ReportIncident,
                    I_GesCaseReports_Id = gcr.I_GesCaseReports_Id,
                    FileName = u.FileName,
                    ServiceName = s.Name,
                    Created = d.Created
                });

        }

        public void DeleteRange(long[] documentIds)
        {
            var documents = _entities.Set<G_ManagedDocuments>().Where(i => documentIds.Contains(i.G_ManagedDocuments_Id));
            _entities.Set<G_ManagedDocuments>().RemoveRange(documents);
        }
    }
}
