using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;

namespace ru.zorro.static_select
{
    public static class SortingExtension
    {


        // не используется 
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool Ascending)
        {
            var asc = source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            var desc = source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));

            if (Ascending)
                return source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            else
                return source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
        }




        // сортировка по указанным пропертям в указанных направлениях
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


        // сортировка по указанной проперти в указанном направлении
        public static IQueryable<T> OrderingHelper<T>(this IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            if (!string.IsNullOrEmpty(propertyName))
                try
                {
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
