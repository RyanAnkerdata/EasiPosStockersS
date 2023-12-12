using EasiPosStockers.Samples;
using Xunit;

namespace EasiPosStockers.EntityFrameworkCore.Applications;

[Collection(EasiPosStockersTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<EasiPosStockersEntityFrameworkCoreTestModule>
{

}
