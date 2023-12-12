using Volo.Abp.Modularity;

namespace EasiPosStockers;

public abstract class EasiPosStockersApplicationTestBase<TStartupModule> : EasiPosStockersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
