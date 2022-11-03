using System;

namespace Sustainalytics.GSS.Entities
{
    public class PrincipleRemark : Remark
    {
        public Guid PrincipleId { get; set; }

        public Principle Principle { get; set; }
    }
}
