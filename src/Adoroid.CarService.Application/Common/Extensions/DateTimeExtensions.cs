using System.Linq.Expressions;

namespace Adoroid.CarService.Application.Common.Extensions;

public static class DateTimeExtensions
{
  public static IQueryable<T> WhereDateIsBetween<T>(this IQueryable<T> queryableSource, Expression<Func<T, DateTime>> expression, DateTime selectedDate)
    {
        var startDate = selectedDate.Date;
        var endDate = selectedDate.Date.AddDays(1);

        var predicate = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.GreaterThanOrEqual(expression.Body, Expression.Constant(startDate)),
                Expression.LessThan(expression.Body, Expression.Constant(endDate))
                ), expression.Parameters);

        return queryableSource.Where(predicate);
    }

    public static IQueryable<T> WhereTwoDateIsBetween<T>(this IQueryable<T> queryableSource, Expression<Func<T, DateTime>> expression, DateTime startDate, DateTime endDate)
    {
        var predicate = Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(
                Expression.GreaterThanOrEqual(expression.Body, Expression.Constant(startDate)),
                Expression.LessThan(expression.Body, Expression.Constant(endDate))
                ), expression.Parameters);

        return queryableSource.Where(predicate);
    }
}
