using System;

namespace Sustainalytics.GSS.Entities
{
    public class CompanyProfileRemark : Remark
    {
        public Guid CompanyProfileId { get; set; }

        public CompanyProfile CompanyProfile { get; set; }
    }
}
