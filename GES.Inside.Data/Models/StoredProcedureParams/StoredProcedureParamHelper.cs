using System.Collections.Generic;

namespace GES.Inside.Data.Models.StoredProcedureParams
{
    public class StoredProcedureParamHelper
    {
        public static IDictionary<string, object> BuildStoredProcedureParams(object obj)
        {
            var paramPairs = new Dictionary<string, object>();
            var properties = obj?.GetType().GetProperties();
            foreach (var property in properties)
            {
                paramPairs[$"@{property.Name}"] = property.GetValue(obj);
            }
            return paramPairs;
        }
    }
}