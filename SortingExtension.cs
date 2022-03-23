using System.Collections.Generic;
using System.Linq;

namespace ru.zorro.static_select
{
    public static class SortingExtension
    {
        public static IEnumerable<T> OrderByDynamic<T>(this IEnumerable<T> source, string propertyName, bool Ascending)
        {
            var asc  = source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            var desc = source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));

            if (Ascending)
                return source.OrderBy(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
            else
                return source.OrderByDescending(x => x.GetType().GetProperty(propertyName).GetValue(x, null));
        }

    }
}
