using Domain.Entities.LogAgg;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Services.Features
{
    [ProviderAlias("Database")]
    public class DbLoggerProvider : ILoggerProvider
    {
        public readonly DbLoggerOption Options;

        public DbLoggerProvider(IOptions<DbLoggerOption> _options)
        {
            Options = _options.Value; // Stores all the options.
        }

        /// <summary>
        /// Creates a new instance of the db logger.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
