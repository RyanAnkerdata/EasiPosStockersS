using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using EasiPosStockers.CostCentres;
using EasiPosStockers.EntityFrameworkCore;
using Xunit;

namespace EasiPosStockers.EntityFrameworkCore.Domains.CostCentres
{
    public class CostCentreRepositoryTests : EasiPosStockersEntityFrameworkCoreTestBase
    {
        private readonly ICostCentreRepository _costCentreRepository;

        public CostCentreRepositoryTests()
        {
            _costCentreRepository = GetRequiredService<ICostCentreRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _costCentreRepository.GetListAsync(
                    costCentreReference: "a8d2d8334e1248348f21",
                    costCentreName: "e479c0279f73445a8ddab5be185409d6db805ac0f37f47c8852d20527ff8ec277cfbf140840346ff9e1932997a9a6b0698e0756d025f49058946157f278da540e37427dc1c49420aab62b107af015d79b748dc19c6644ba7a26379b16e7c86c382443d71",
                    isDisabled: true
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _costCentreRepository.GetCountAsync(
                    costCentreReference: "4ba27d4e24bb497aa995",
                    costCentreName: "8512bb1185b44447b9c8a4d4e9342932a6df7e752faf48dc9fcde8b877259fe66f3aab33a0a041e1a141b433a6ff33fd56906bddfdf142eea31e6f315154a9052a54d23586d4415d8364e16d862e625710f3c7829aed478faceaecc69454d1e1c3309d88",
                    isDisabled: true
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}