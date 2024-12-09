using System.Linq.Expressions;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// Методы работы с <see cref="Expression"/>.
/// </summary>
public sealed partial class BooksRepository
{
    private static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, string>> valueSelector,
        Expression<Func<string, bool>> condition)
    {
        var parameter = valueSelector.Parameters[0];

        var body = Expression.Invoke(condition, valueSelector.Body);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression<Func<string, bool>> BuildStringContainsExpression(string keyword)
    {
        var processedKeyword = keyword.Replace("_", string.Empty).ToLower();
        var parameter = Expression.Parameter(typeof(string), "value");

        var valueProcessed = Expression.Call(
            Expression.Call(parameter, nameof(string.Replace), null, Expression.Constant("_"), Expression.Constant(string.Empty)),
            nameof(string.ToLower), null);

        var body = Expression.OrElse(
            Expression.Call(valueProcessed, nameof(string.Contains), null, Expression.Constant(processedKeyword)),
            Expression.Call(Expression.Constant(processedKeyword), nameof(string.Contains), null, valueProcessed));

        return Expression.Lambda<Func<string, bool>>(body, parameter);
    }
}