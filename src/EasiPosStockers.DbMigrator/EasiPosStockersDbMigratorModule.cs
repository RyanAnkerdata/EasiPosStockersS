using EasiPosStockers.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace EasiPosStockers.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EasiPosStockersEntityFrameworkCoreModule),
    typeof(EasiPosStockersApplicationContractsModule)
)]
public class EasiPosStockersDbMigratorModule : AbpModule
{
}
