using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace ru.zorro.static_select
{
    public static class SortingExtension
    {
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool Ascending)
        {
            var asc = source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            var desc = source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));

            if (Ascending)
                return source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            else
                return source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
        }


        public static IQueryable<T> OrderingHelper<T>(this IQueryable<T> source, string[] propertyName, bool[] descending)
        {
            bool anotherLevel = false;
            foreach (var property in propertyName)
            {
                source = source.OrderingHelper<T>(property, true, anotherLevel);
                anotherLevel = true;
            }
            return source;
        }


        public static IQueryable<T> OrderingHelper<T>(this IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            if (!string.IsNullOrEmpty(propertyName))
                try
                {
                    // --------------
                    string sss = (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty);
                    // ---------------
                    ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
                    MemberExpression property = Expression.PropertyOrField(param, propertyName);
                    LambdaExpression sort = Expression.Lambda(property, param);

                    MethodCallExpression call = Expression.Call(
                        typeof(Queryable),
                        (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                        new[] { typeof(T), property.Type },
                        source.Expression,
                        Expression.Quote(sort));
                    return (IQueryable<T>)source.Provider.CreateQuery<T>(call);
                }
                catch
                {
                    return null;
                }
            return null;
        }


    }
}
