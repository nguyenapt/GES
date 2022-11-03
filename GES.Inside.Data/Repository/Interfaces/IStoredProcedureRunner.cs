using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IStoredProcedureRunner
    {

        IEnumerable<T> Execute<T>(string storedProcedureName, IDictionary<string, object> sqlParams, string[] cacheTags) where T : class;
    }
}
