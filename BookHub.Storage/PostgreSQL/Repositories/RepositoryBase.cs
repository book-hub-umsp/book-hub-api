using System.Linq.Expressions;

using BookHub.Storage.PostgreSQL.Abstractions;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// База для репозиториев.
/// </summary>
public abstract class RepositoryBase
{
    public IRepositoryContext Context { get; }

    protected RepositoryBase(IRepositoryContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    protected static Expression<Func<TData, bool>> CreateFilterWithLeftAndRightBound<TData, TKey>(
        Expression<Func<TData, TKey>> selector,
        TKey leftValue,
        TKey rightValue)
    {
        var parameter = Expression.Parameter(typeof(TData));
        var expressionParameter = Expression.Property(parameter, GetParameterName(selector));

        var body1 = Expression.GreaterThanOrEqual(expressionParameter, Expression.Constant(leftValue, typeof(TKey)));
        var body2 = Expression.LessThanOrEqual(expressionParameter, Expression.Constant(rightValue, typeof(TKey)));

        return Expression.Lambda<Func<TData, bool>>(Expression.AndAlso(body1, body2), parameter);
    }

    private static string GetParameterName<TData, TKey>(Expression<Func<TData, TKey>> expression)
    {
        if (expression.Body is not MemberExpression memberExpression)
        {
            memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
        }

        return memberExpression!.ToString().Substring(2);
    }
}