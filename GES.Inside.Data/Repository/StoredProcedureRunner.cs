using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Common.Exceptions;
using GES.Common.Services.Interface;

namespace GES.Inside.Data.Repository
{
    public class StoredProcedureRunner : IStoredProcedureRunner
    {
        private readonly GesEntities _dbContext;
        private readonly IApplicationSettingsService _applicationSettingsService;
        protected readonly IGesLogger Logger;

        public StoredProcedureRunner(GesEntities dbContext, IApplicationSettingsService applicationSettingsService, IGesLogger logger)
        {
            _dbContext = dbContext;
            _applicationSettingsService = applicationSettingsService;
            Logger = logger;
        }
        
        public IEnumerable<T> Execute<T>(string storedProcedureName, IDictionary<string, object> sqlParams, string[] cacheTags = null) where T : class 
        {
            _dbContext.Database.CommandTimeout = _applicationSettingsService.LongCommandTimeout;
            var customCacheKey = GetCustomCacheKey(storedProcedureName, sqlParams);
            var query = GetDataFromStoredProcedure<T>(_dbContext, storedProcedureName, sqlParams);
            
            try
            {
                return query.FromCache(customCacheKey, cacheTags ?? new string[0]);
            }
            catch(SqlException ex)
            {
                // Write log
                Logger.Error(ex, $"Exception when execute the store {storedProcedureName}. The error {ex.Message}. Please check inner exception for detail");

                throw new GesDbException($"Error when execute the store {storedProcedureName}", ex);
            }
            catch (System.Data.DataException ex)
            {
                // Write log
                Logger.Error(ex, $"Exception when execute the store {storedProcedureName}. The error {ex.Message}. Please check inner exception for detail");

                throw new GesDbException($"Error when execute the store {storedProcedureName}", ex);
            }
        }

        private IQueryable<T> GetDataFromStoredProcedure<T>(DbContext dbContext, string storedProcedureName, IDictionary<string, object> paramPairs)
        {
            var sql = $"{storedProcedureName}";
            var sqlParams = new List<object>();
            if (paramPairs != null && paramPairs.Any())
            {
                foreach (var param in paramPairs)
                {
                    sql += $" {param.Key},";
                    sqlParams.Add(new SqlParameter(param.Key, param.Value));
                }
            }

            sql = sql.EndsWith(",", System.StringComparison.Ordinal) ? sql.TrimEnd(',') : sql;

            return dbContext.Database.SqlQuery<T>(sql, sqlParams.ToArray()).AsQueryable();
        }

        private string GetCustomCacheKey(string queryName, IDictionary<string, object> paramPairs)
        {
            var customCacheKey = $"{queryName}_";
            if (paramPairs != null && paramPairs.Any())
            {
                foreach (var param in paramPairs)
                {
                    customCacheKey += $"{param.Key};{param.Value}";
                }
            }
            return customCacheKey;
        }
    }
}
