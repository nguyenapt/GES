using System;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using System.Collections.Generic;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class OrganizationDocumentService : EntityService<GesEntities, DataContexts.G_Organizations_GesDocument>, IOrganizationDocumentService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_Organizations_GesDocumentRepository _organizationDocumentsReporsitory;

        public OrganizationDocumentService(IUnitOfWork<GesEntities> unitOfWork, IG_Organizations_GesDocumentRepository organizationDocumentsReporsitory, IGesLogger logger) : base(unitOfWork, logger, organizationDocumentsReporsitory)
        {
            _dbContext = (GesEntities)unitOfWork.DbContext;
            _organizationDocumentsReporsitory = organizationDocumentsReporsitory;
        }

        /// <summary>
        /// Clear portfolio_Companies by portfolioId
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public bool RemoveOrganizationDocumentByDocumentId(Guid documentId)
        {
            this.SafeExecute(() => this._organizationDocumentsReporsitory.BatchDelete(d => d.GesDocumentId == documentId));

            return true;
        }

        public void AddBatch(List<G_Organizations_GesDocument> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }
            
            this.SafeExecute(() => _organizationDocumentsReporsitory.AddBatch(entities));
        }

    }
}