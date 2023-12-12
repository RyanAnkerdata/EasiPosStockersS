using EasiPosStockers.Samples;
using Xunit;

namespace EasiPosStockers.EntityFrameworkCore.Domains;

[Collection(EasiPosStockersTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<EasiPosStockersEntityFrameworkCoreTestModule>
{

}
