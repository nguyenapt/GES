namespace Sustainalytics.GSS.Entities
{
    public class CorporateTreeRemark : Remark
    {
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
