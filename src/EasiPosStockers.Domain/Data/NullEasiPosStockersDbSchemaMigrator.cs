using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EasiPosStockers.Data;

/* This is used if database provider does't define
 * IEasiPosStockersDbSchemaMigrator implementation.
 */
public class NullEasiPosStockersDbSchemaMigrator : IEasiPosStockersDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
