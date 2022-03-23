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

        public static IQueryable<DatatypeTest> FilterByProps(this IQueryable<DatatypeTest> query,
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

        public static IQueryable<DatatypeTest> FilterByBool(this IQueryable<DatatypeTest> query,
                                                  bool search)
        {
            return query.Where(x => x.DatatypeBool == search);
        }

        public static IQueryable<DatatypeTest> FilterByString(this IQueryable<DatatypeTest> query,
                                                  string search)
        {
            return query.Where(x => x.DatatypeString.IndexOf(search) != -1);
        }

        public static IQueryable<DatatypeTest> FilterByInt(this IQueryable<DatatypeTest> query,
                                                  int search)
        {
            return query.Where(x => x.DatatypeInt == search);
        }
    }
}
