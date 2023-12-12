using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EasiPosStockers.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class EasiPosStockersDbContextFactory : IDesignTimeDbContextFactory<EasiPosStockersDbContext>
{
    public EasiPosStockersDbContext CreateDbContext(string[] args)
    {
        EasiPosStockersEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<EasiPosStockersDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new EasiPosStockersDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../EasiPosStockers.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
