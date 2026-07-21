using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace SmartCare.Repository.Database;

public static class SqliteConnectionFactory
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var configuredConnection = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(configuredConnection))
            throw new InvalidOperationException("ConnectionStrings:DefaultConnection is not configured.");

        var builder = new SqliteConnectionStringBuilder(configuredConnection);
        if (string.IsNullOrWhiteSpace(builder.DataSource))
            throw new InvalidOperationException("DefaultConnection must specify a SQLite Data Source.");

        if (!Path.IsPathRooted(builder.DataSource))
            builder.DataSource = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, builder.DataSource));

        return builder.ConnectionString;
    }

    public static SqliteConnection Create(IConfiguration configuration) =>
        new(GetConnectionString(configuration));
}
