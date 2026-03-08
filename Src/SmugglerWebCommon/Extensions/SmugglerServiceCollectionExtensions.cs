using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmugglerWebCommon.Data;
using SmugglerWebCommon.Security;

namespace SmugglerWebCommon.Extensions;

public static class SmugglerServiceCollectionExtensions
{
    public static IServiceCollection AddSmugglerDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' is missing.");

        services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddScoped<IDbConnection>(sp => sp.GetRequiredService<IDbConnectionFactory>().CreateConnection());

        return services;
    }

    public static IServiceCollection AddSmugglerRsa(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new RsaKeyOptions
        {
            PublicKeyPem = configuration["Security:RSA:PublicKeyPem"] ?? string.Empty,
            PrivateKeyPem = configuration["Security:RSA:PrivateKeyPem"] ?? string.Empty
        };

        services.AddSingleton(options);
        services.AddSingleton<IRsaCryptoService, RsaCryptoService>();

        return services;
    }
}