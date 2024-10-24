using BookHub.Storage.PostgreSQL.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Фабрика для создания scoped единиц работы.
/// </summary>
public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
{
    public UnitOfWorkFactory(IServiceScopeFactory scopeFactory) 
    { 
        _scopeFactory = scopeFactory
            ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public IBooksUnitOfWork Create()
    {
        var scope = _scopeFactory.CreateScope();

        return scope.ServiceProvider.GetRequiredService<IBooksUnitOfWork>();
    }

    private readonly IServiceScopeFactory _scopeFactory;
}