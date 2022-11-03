using System.ComponentModel;

namespace Sustainalytics.GSS.Entities
{
    public enum NormType
    {
        [Description("Human Rights")]
        HumanRights = 1,
        [Description("Labour Rights")]
        LabourRights = 2,
        Environment = 3,
        [Description("Business Ethics")]
        BusinessEthics = 4
    }
}
