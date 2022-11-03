using System;
using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesUNGPAssessmentFormResourcesRepository : IGenericRepository<GesUNGPAssessmentFormResources>
    {
        IEnumerable<GesUNGPAssessmentFormResources> GetGesUNGPAssessmentFormResourcesByFormId(Guid formId);
        GesUNGPAssessmentFormResources GetById(Guid id);
    }
}
