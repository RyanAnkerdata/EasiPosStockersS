using Xunit;

namespace EasiPosStockers.EntityFrameworkCore;

[CollectionDefinition(EasiPosStockersTestConsts.CollectionDefinitionName)]
public class EasiPosStockersEntityFrameworkCoreCollection : ICollectionFixture<EasiPosStockersEntityFrameworkCoreFixture>
{

}
