using Microsoft.Extensions.Logging;
using Storage.PostgreSQL.Abstractions;

namespace Storage.PostgreSQL;

public sealed class RepositoryContext : IRepositoryContext
{
    public RepositoryContext(
        BooksContext context, 
        ILogger<RepositoryContext> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SaveChanges()
    {
        _logger.LogDebug("Save intermediate changes");

        _ = _context.SaveChanges();

        _logger.LogDebug("Intermediate changes sent to DB");
    }

    private readonly ILogger<RepositoryContext> _logger;
    private readonly BooksContext _context;
}