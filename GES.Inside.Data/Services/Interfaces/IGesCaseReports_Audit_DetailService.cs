using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IGesCaseReports_Audit_DetailService : IEntityService<GesCaseReports_Audit_Details>
    {
        GesCaseReports_Audit_Details GetById(Guid id);

        List<GesCaseReports_Audit_Details> GetGesCaseProfileAuditByCaseReportId(long caseReportId, string columnName);
    }
}
