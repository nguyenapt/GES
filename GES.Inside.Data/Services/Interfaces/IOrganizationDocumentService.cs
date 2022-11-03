using System;
using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IOrganizationDocumentService : IEntityService<DataContexts.G_Organizations_GesDocument>
    {
        bool RemoveOrganizationDocumentByDocumentId(Guid documentId);
        void AddBatch(List<G_Organizations_GesDocument> entities);
    }
}
