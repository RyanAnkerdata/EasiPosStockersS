using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EasiPosStockers.Data;
using Volo.Abp.DependencyInjection;

namespace EasiPosStockers.EntityFrameworkCore;

public class EntityFrameworkCoreEasiPosStockersDbSchemaMigrator
    : IEasiPosStockersDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEasiPosStockersDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the EasiPosStockersDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<EasiPosStockersDbContext>()
            .Database
            .MigrateAsync();
    }
}
