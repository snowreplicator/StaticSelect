using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ru.zorro.static_select
{

    // http://www.albahari.com/nutshell/predicatebuilder.aspx
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }





        // https://entityframework.net/knowledge-base/39182903/how-to-construct-where-expression-dynamically-in-entity-framework-
        public static IQueryable<T> WhereEquals<T>(this IQueryable<T> source, string member, object value)
        {
            var item = Expression.Parameter(typeof(T), "item");
            var memberValue = member.Split('.').Aggregate((Expression)item, Expression.PropertyOrField);
            var memberType = memberValue.Type;
            if (value != null && value.GetType() != memberType)
                value = Convert.ChangeType(value, memberType);
            var condition = Expression.Equal(memberValue, Expression.Constant(value, memberType));
            var predicate = Expression.Lambda<Func<T, bool>>(condition, item);
            return source.Where(predicate);
        }

        // https://entityframework.net/knowledge-base/39182903/how-to-construct-where-expression-dynamically-in-entity-framework-
        public static IQueryable<T> WhereEquals_2<T>(this IQueryable<T> source, string propertyName, object value)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return null;
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "item");
            Expression whereProperty = Expression.Property(parameter, propertyName);
            Expression constant = Expression.Constant(value);
            Expression condition = Expression.Equal(whereProperty, constant);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
            return source.Where(lambda);
        }


    }
}