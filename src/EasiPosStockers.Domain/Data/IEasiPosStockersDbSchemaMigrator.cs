using System.Threading.Tasks;

namespace EasiPosStockers.Data;

public interface IEasiPosStockersDbSchemaMigrator
{
    Task MigrateAsync();
}
