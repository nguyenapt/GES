using System.ComponentModel;

namespace Sustainalytics.GSS.Entities
{
    public enum BasisForNonCompliance
    {
        [Description("Official Source")]
        OfficialSource = 1,

        [Description("Company Admittance of Guilt")]
        CompanyAdmittanceOfGuilt = 2,

        [Description("Multiple Independent Sources")]
        MultipleIndependentSources = 3
    }
}
