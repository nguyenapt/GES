using System.Collections.Generic;

namespace GES.Inside.Data.Models.StoredProcedureParams
{
    public abstract class StoredProcedureParams
    {
        public IDictionary<string, object> BuildStoredProcedureParams() => StoredProcedureParamHelper.BuildStoredProcedureParams(this);
    }
}
