using Volo.Abp.Modularity;

namespace EasiPosStockers;

[DependsOn(
    typeof(EasiPosStockersDomainModule),
    typeof(EasiPosStockersTestBaseModule)
)]
public class EasiPosStockersDomainTestModule : AbpModule
{

}
