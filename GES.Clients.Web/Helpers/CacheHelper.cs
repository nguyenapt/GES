using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Script.Serialization;
using GES.Clients.Web.Configs;
using Z.EntityFramework.Plus;

namespace GES.Clients.Web.Helpers
{
    public static class CacheHelper
    {
        #region Common
        public static void ClearAllCache(List<long> toBeClearedIndividualIds, List<long> toBeClearedOrgIds)
        {
            // clear all second-level cache
            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
            }

            // clear OutputCache
            // generate lists of unique keys
            var individualUniqueKeys = toBeClearedIndividualIds.Select(i => "individual" + i).ToList();
            var orgUniqueKeys = toBeClearedOrgIds.Select(i => "organization" + i).ToList();
            individualUniqueKeys.ForEach(CacheHelper.ClearPreDefinedOutputCache);
            orgUniqueKeys.ForEach(CacheHelper.ClearPreDefinedOutputCache);
        }

        public static void ClearFocusListRelatedCache(long individualId)
        {
            var suffixIndividual = "-i" + individualId;

            // clear focusList-related second-level cache
            QueryCacheManager.ExpireTag("CompanySearch" + suffixIndividual);
            QueryCacheManager.ExpireTag("GesCaseReport" + suffixIndividual);
            QueryCacheManager.ExpireTag("NumberOfCases" + suffixIndividual);
            QueryCacheManager.ExpireTag("FocusList" + suffixIndividual);

            // clear focusList-related output cache
            var uniqueKey = string.Format("individual{0}", individualId);
            ClearPreDefinedOutputCache(uniqueKey);
        }
        #endregion

        #region OutputCache
        public static void ClearPreDefinedOutputCache(string uniqueKey)
        {
            var toBeClearedActionMethods = SiteSettings.ToBeClearedActionMethods;
            foreach (var record in toBeClearedActionMethods)
            {
                var url = string.Format("/{0}/{1}", record, uniqueKey);
                ClearOutputCacheAtUrl(url);
            }
        }

        public static void ClearOutputCacheAtUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                HttpResponse.RemoveOutputCacheItem(url);
            }
        }

        public static void ClearOutputCacheWithUniqueKey(string path, string uniqueKey)
        {
            var fullUrl = string.Format("/{0}/{1}", path, uniqueKey);
            ClearOutputCacheAtUrl(fullUrl);
        }
        #endregion

        #region Second-Level Cache
        
        #endregion
    }
}