namespace Domain.Entities.LogAgg;

public class DbLoggerOption
{
    public string ConnectionString { get; init; }

    public string[] LogFields { get; init; }

    public string LogTable { get; init; }

    public DbLoggerOption()
    {
    }
}