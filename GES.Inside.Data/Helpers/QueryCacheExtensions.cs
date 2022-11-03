using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Helpers
{
    public static class QueryCacheExtensions
    {
        static void AddCacheTag(string cacheKey, params string[] tags)
        {
            foreach (var tag in tags)
                QueryCacheManager.CacheTags.AddOrUpdate(QueryCacheManager.CachePrefix + tag, x => new List<string>()
                {
                    cacheKey
                }, (x, list) =>
                {
                    if (!list.Contains(cacheKey))
                        list.Add(cacheKey);
                    return list;
                });
        }

        public static IEnumerable<T> FromCache<T>(this IQueryable<T> query, string customCacheKey, string[] tags) where T : class
        {
            var obj = QueryCacheManager.Cache.Get(customCacheKey);
            if (obj == null)
            {
                var list = (object)query.AsNoTracking().ToList();
                obj = QueryCacheManager.Cache.AddOrGetExisting(customCacheKey, list, QueryCacheManager.DefaultCacheItemPolicy) ?? list;

                if (tags != null && tags.Length > 0)
                    AddCacheTag(customCacheKey, tags);
            }
            return (IEnumerable<T>)obj;
        }
    }
}
