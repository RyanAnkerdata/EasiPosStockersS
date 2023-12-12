using Volo.Abp.Modularity;

namespace EasiPosStockers;

[DependsOn(
    typeof(EasiPosStockersApplicationModule),
    typeof(EasiPosStockersDomainTestModule)
)]
public class EasiPosStockersApplicationTestModule : AbpModule
{

}
