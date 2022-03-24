using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ru.zorro.static_select
{
    // https://sprosi.pro/questions/2211961/dinamicheskie-usloviya-v-zaprose-linq-to-entities
    public static class SearchExtension
    {

        public static IQueryable<DatatypeTests> FilterByProps(this IQueryable<DatatypeTests> query,
            string search, string[] propertyName)
        {
            foreach (var property in propertyName)
            {
                switch (property)
                {
                    case "DatatypeBool":
                        try
                        {
                            query = FilterByBool(query, bool.Parse(search));
                        }
                        catch (Exception ex)
                        {
                        }
                        break;

                    case "DatatypeString":
                        query = FilterByString(query, search);
                        break;

                    case "DatatypeInt":
                        try
                        {
                            query = FilterByInt(query, int.Parse(search));
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                }

            }
            return query;
        }

        public static IQueryable<DatatypeTests> FilterByBool(this IQueryable<DatatypeTests> query,
                                                  bool search)
        {
            return query.Where(x => x.DatatypeBool == search);
        }

        public static IQueryable<DatatypeTests> FilterByString(this IQueryable<DatatypeTests> query,
                                                  string search)
        {
            return query.Where(x => x.DatatypeString.IndexOf(search) != -1);
        }

        public static IQueryable<DatatypeTests> FilterByInt(this IQueryable<DatatypeTests> query,
                                                  int search)
        {
            return query.Where(x => x.DatatypeInt == search);
        }


        public static Expression<Func<TIn, TOut>> Compose<TIn, TInOut, TOut>(
        this Expression<Func<TIn, TInOut>> input,
        Expression<Func<TInOut, TOut>> inOutOut)
        {
            // это параметр x => blah-blah. Для лямбды нам нужен null
            var param = Expression.Parameter(typeof(TIn), null);
            // получаем объект, к которому применяется выражение
            var invoke = Expression.Invoke(input, param);
            // и выполняем "получи объект и примени к нему его выражение"
            var res = Expression.Invoke(inOutOut, invoke);

            // возвращаем лямбду нужного типа
            return Expression.Lambda<Func<TIn, TOut>>(res, param);
        }

        // Добавляем "продвинутый" вариант Where
        public static IQueryable<T> WhereHelper2<T, TParam>(this IQueryable<T> queryable,
            Expression<Func<T, TParam>> prop, Expression<Func<TParam, bool>> where)
        {
            return queryable.Where(prop.Compose(where));
        }

    }
}
