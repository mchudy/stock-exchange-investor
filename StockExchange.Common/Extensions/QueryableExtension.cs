using StockExchange.Common.LinqUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace StockExchange.Common.Extensions
{
    public static class QueryableExtension
    {
        private sealed class ValueContainer
        {
            public object Value { get; set; }
        }

        public static IQueryable<object> Select<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);
            var containerType = typeof(ValueContainer);
            var ctor = Expression.New(containerType);
            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);
            var displayValueProperty = containerType.GetProperty("Value");
            var displayValueAssignment = Expression.Bind(displayValueProperty, property);
            var memberInit = Expression.MemberInit(ctor, displayValueAssignment);
            var valueProperty = Expression.Property(memberInit, "Value");
            var selector = Expression.Lambda(valueProperty, arg);
            return query.Provider.CreateQuery<object>(Expression.Call(typeof(Queryable), "Select", new[] { entityType, typeof(object) }, query.Expression, selector));
        }

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, List<OrderBy> orderBys)
        {
            var first = true;
            IOrderedQueryable<TSource> result = null;
            if (orderBys == null) return query;
            foreach (var orderBy in orderBys)
            {
                if (first)
                {
                    result = orderBy.Desc ? query.OrderByDescending(orderBy.Column) : query.OrderBy(orderBy.Column);
                    first = false;
                }
                else
                {
                    result = orderBy.Desc ? result.ThenByDescending(orderBy.Column) : result.ThenBy(orderBy.Column);
                }
            }
            return result ?? query;
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var sortOrder = AttributeHelper.GetPropertyAttribute<SortOrderAttribute>(typeof(TSource), propertyName);
            if (sortOrder == null) return OrderBy(query, propertyName, "OrderBy");
            IOrderedQueryable<TSource> result = null;
            var first = true;
            foreach (var order in sortOrder.Orders)
            {
                if (first)
                {
                    result = OrderBy(query, order, "OrderBy");
                    first = false;
                }
                else
                {
                    result = OrderBy(result, order, "ThenBy");
                }
            }
            return result;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var sortOrder = AttributeHelper.GetPropertyAttribute<SortOrderAttribute>(typeof(TSource), propertyName);
            if (sortOrder == null) return OrderBy(query, propertyName, "OrderByDescending");
            IOrderedQueryable<TSource> result = null;
            var first = true;
            foreach (var order in sortOrder.Orders)
            {
                if (first)
                {
                    result = OrderBy(query, order, "OrderByDescending");
                    first = false;
                }
                else
                {
                    result = OrderBy(result, order, "ThenByDescending");
                }
            }
            return result;
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var sortOrder = AttributeHelper.GetPropertyAttribute<SortOrderAttribute>(typeof(TSource), propertyName);
            if (sortOrder == null) return OrderBy(query, propertyName, "ThenBy");
            IOrderedQueryable<TSource> result = null;
            var first = true;
            foreach (var order in sortOrder.Orders)
            {
                if (first)
                {
                    result = OrderBy(query, order, "ThenBy");
                    first = false;
                }
                else
                {
                    result = OrderBy(result, order, "ThenBy");
                }
            }
            return result;
        }

        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            var sortOrder = AttributeHelper.GetPropertyAttribute<SortOrderAttribute>(typeof(TSource), propertyName);
            if (sortOrder == null) return OrderBy(query, propertyName, "ThenByDescending");
            IOrderedQueryable<TSource> result = null;
            var first = true;
            foreach (var order in sortOrder.Orders)
            {
                if (first)
                {
                    result = OrderBy(query, order, "ThenByDescending");
                    first = false;
                }
                else
                {
                    result = OrderBy(result, order, "ThenByDescending");
                }
            }
            return result;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName, string orderMethod)
        {
            var entityType = typeof(TSource);
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo == null) throw new ArgumentException();
            var arg = Expression.Parameter(entityType, "x");
            var property = Expression.Property(arg, propertyName);
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            LambdaExpression selector;
            Type type;
            if (propertyType.IsEnum)
            {
                var toString = typeof(object).GetMethod("ToString");
                var toStringValue = Expression.Call(property, toString);
                type = typeof(string);
                selector = Expression.Lambda(toStringValue, arg);
            }
            else
            {
                type = propertyInfo.PropertyType;
                selector = Expression.Lambda(property, arg);
            }
            return (IOrderedQueryable<TSource>)query.Provider.CreateQuery<TSource>(Expression.Call(typeof(Queryable), orderMethod, new[] { typeof(TSource), type }, query.Expression, Expression.Quote(selector)));
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> query, IEnumerable<SearchBy> searchBys)
        {
            return searchBys != null ? searchBys.Aggregate(query, (current, search) => current.Where(search.Field, search.Values)) : query;
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> query, string propertyName, IEnumerable<string> values)
        {
            var entityType = typeof(TSource);
            var arg = Expression.Parameter(entityType, "x");
            var enumerable = values.ToList();
            var first = enumerable.FirstOrDefault();
            if (first == null) return query;
            var expression = GetWhereExpression(entityType, arg, propertyName, first);
            expression = enumerable.Skip(1).Aggregate(expression, (current, value) => Expression.Or(current, GetWhereExpression(entityType, arg, propertyName, value)));
            var selector = Expression.Lambda(expression, arg);
            return query.Provider.CreateQuery<TSource>(Expression.Call(typeof(Queryable), "Where", new[] { typeof(TSource) }, query.Expression, selector));
        }

        private static Expression GetWhereExpression(Type entityType, ParameterExpression parameter, string propertyName, string value)
        {
            var propertyInfo = entityType.GetProperty(propertyName);
            if (propertyInfo == null) throw new ArgumentException();
            var property = Expression.Property(parameter, propertyName);
            object convertedValue = null;
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            if (propertyType.IsEnum)
            {
                var method = typeof(EnumExtension).GetMethod("GetEnumValueByDescription");
                var genericMethod = method.MakeGenericMethod(propertyInfo.PropertyType);
                convertedValue = genericMethod.Invoke(null, new object[] { value });
                if (convertedValue == null)
                {
                    if (Enum.GetNames(propertyType).Contains(value))
                    {
                        convertedValue = Enum.Parse(propertyType, value);
                    }
                }
            }
            if (convertedValue == null)
            {
                try
                {
                    convertedValue = (string.IsNullOrEmpty(value) && propertyInfo.PropertyType == typeof(string)) ? value : Convert.ChangeType(value, propertyType);
                }
                catch
                {
                    convertedValue = null;
                }
            }
            Expression resultExpression;
            if (propertyInfo.PropertyType == typeof(string) && convertedValue != null && convertedValue.ToString() != "")
            {
                var constant = Expression.Constant(convertedValue);
                var contains = typeof(string).GetMethod("Contains");
                resultExpression = Expression.Call(property, contains, constant);
            }
            else
            {
                var constant = Expression.Constant(convertedValue);
                var valueExpression = Expression.Convert(constant, propertyInfo.PropertyType);
                resultExpression = Expression.Equal(property, valueExpression);
            }
            return resultExpression;
        }
    }
}
