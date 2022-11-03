using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GES.Clients.Web.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// The method ensure that will initialize new empty list when the input is null or all items of list are default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> EnsureListNotNullOrAllItemsAreDefault<T>(this List<T> list)
        {
            if (list == null || list.All(i => EqualityComparer<T>.Default.Equals(i, default(T))))
            {
                return new List<T>();
            }

            return list;
        }
    }
}