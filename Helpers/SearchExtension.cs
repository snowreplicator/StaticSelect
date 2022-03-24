using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace ru.zorro.static_select
{
    public static class SearchExtension
    {

        // https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/concepts/expression-trees/how-to-use-expression-trees-to-build-dynamic-queries
        // поиск указанной подстроки во всех текстовых пропертях списка
        public static IQueryable<T> TextFilter<T>(this IQueryable<T> source, string term)
        {
            if (string.IsNullOrEmpty(term)) { return source; }

            // T is a compile-time placeholder for the element type of the query.
            Type elementType = typeof(T);

            // Get all the string properties on this specific type.
            System.Reflection.PropertyInfo[] stringProperties =
                elementType.GetProperties()
                    .Where(x => x.PropertyType == typeof(string))
                    .ToArray();
            if (!stringProperties.Any()) { return source; }

            // Get the right overload of String.Contains
            MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            // Create a parameter for the expression tree:
            // the 'x' in 'x => x.PropertyName.Contains("term")'
            // The type of this parameter is the query's element type
            ParameterExpression prm = Expression.Parameter(elementType);

            // Map each property to an expression tree node
            IEnumerable<Expression> expressions = stringProperties
                .Select(prp =>
                    // For each property, we have to construct an expression tree node like x.PropertyName.Contains("term")
                    Expression.Call(                  // .Contains(...) 
                        Expression.Property(          // .PropertyName
                            prm,           // x 
                            prp
                        ),
                        containsMethod,
                        Expression.Constant(term)     // "term" 
                    )
                );

            // Combine all the resultant expression nodes using ||
            Expression body = expressions
                .Aggregate(
                    (prev, current) => Expression.Or(prev, current)
                );

            // Wrap the expression body in a compile-time-typed lambda expression
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, prm);

            // Because the lambda is compile-time-typed (albeit with a generic parameter), we can use it with the Where method
            return source.Where(lambda);
        }





        public static IQueryable<T> SearchHelper<T>(this IQueryable<T> source, string searchString)
        {
            Type elementType = typeof(T);

            PropertyInfo[] elementProperties = elementType.GetProperties()
                .Where(x => x.PropertyType == typeof(string) ||
                            x.PropertyType == typeof(int) ||
                            x.PropertyType == typeof(long))
                .ToArray();

            if (!elementProperties.Any())
                return source;

            
            HashSet<Type> types = new HashSet<Type>();
            foreach (PropertyInfo elementProperty in elementProperties)
            {
                types.Add(elementProperty.PropertyType);
            }

            foreach (Type type in types)
            {
                object term = searchString;
                if (type == typeof(int)) term = int.Parse(searchString);
                if (type == typeof(long)) term = long.Parse(searchString);


                var source2 = SearchHelper<T>(source, type, term);
                Console.WriteLine("\nprop searc:");
                foreach (var model in source2)
                    Console.WriteLine(JsonConvert.SerializeObject(model));


                source.Concat(source2);
            }


            return source;
            //return SearchHelper<T>(source, type, term);
        }


        public static IQueryable<T> SearchHelper<T>(this IQueryable<T> source, Type type, object term)
        {
            //if (string.IsNullOrEmpty(term)) { return source; }
            if (type == null || term == null)
                return source;


            // T is a compile-time placeholder for the element type of the query.
            Type elementType = typeof(T);


            // Get all the string properties on this specific type.
            System.Reflection.PropertyInfo[] stringProperties =
                elementType.GetProperties()
                    .Where(x => x.PropertyType == type)/* ||
                    x.PropertyType == typeof(string) ||
                                x.PropertyType == typeof(int) ||
                                x.PropertyType == typeof(long))*/
                    .ToArray();
            if (!stringProperties.Any()) { return source; }


            //int term2 = int.Parse(term);
            //object term3 = int.Parse(term);
            //object term3 = term;

            // Get the right overload of String.Contains
            //MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            //MethodInfo containsMethod = typeof(int).GetMethod("Equals", new[] { typeof(int) })!;
            MethodInfo containsMethod = null;
            if (type == typeof(string))
                containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
            if (type == typeof(int))
                containsMethod = typeof(int).GetMethod("Equals", new[] { typeof(int) })!;
            if (type == typeof(long))
                containsMethod = typeof(long).GetMethod("Equals", new[] { typeof(long) })!;
            if (containsMethod == null)
                return source;


            // Create a parameter for the expression tree:
            // the 'x' in 'x => x.PropertyName.Contains("term")'
            // The type of this parameter is the query's element type
            ParameterExpression prm = Expression.Parameter(elementType);

            // Map each property to an expression tree node
            IEnumerable<Expression> expressions = stringProperties
                .Select(prp =>
                    // For each property, we have to construct an expression tree node like x.PropertyName.Contains("term")
                    Expression.Call(                  // .Contains(...) 
                        Expression.Property(          // .PropertyName
                            prm,           // x 
                            prp
                        ),
                        containsMethod,
                        Expression.Constant(term)     // "term" 
                    )
                );

            // Combine all the resultant expression nodes using ||
            Expression body = expressions
                .Aggregate(
                    (prev, current) => Expression.Or(prev, current)
                );

            // Wrap the expression body in a compile-time-typed lambda expression
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(body, prm);

            // Because the lambda is compile-time-typed (albeit with a generic parameter), we can use it with the Where method
            return source.Where(lambda);
        }
    }
}
