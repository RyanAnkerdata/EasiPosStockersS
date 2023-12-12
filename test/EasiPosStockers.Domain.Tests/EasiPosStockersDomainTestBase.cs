using Volo.Abp.Modularity;

namespace EasiPosStockers;

/* Inherit from this class for your domain layer tests. */
public abstract class EasiPosStockersDomainTestBase<TStartupModule> : EasiPosStockersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
