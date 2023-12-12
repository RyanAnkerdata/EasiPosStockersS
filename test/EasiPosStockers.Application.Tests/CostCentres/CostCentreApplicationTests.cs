using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentresAppServiceTests<TStartupModule> : EasiPosStockersApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly ICostCentresAppService _costCentresAppService;
        private readonly IRepository<CostCentre, Guid> _costCentreRepository;

        public CostCentresAppServiceTests()
        {
            _costCentresAppService = GetRequiredService<ICostCentresAppService>();
            _costCentreRepository = GetRequiredService<IRepository<CostCentre, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _costCentresAppService.GetListAsync(new GetCostCentresInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.CostCentre.Id == Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b")).ShouldBe(true);
            result.Items.Any(x => x.CostCentre.Id == Guid.Parse("99f9495b-6f6d-4a61-bd64-cc3c63086af0")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _costCentresAppService.GetAsync(Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new CostCentreCreateDto
            {
                CostCentreReference = "7b2237251968459784bd",
                CostCentreName = "e558dfcea31d478992479dd4c2aa0f21ffa07fe1d16c48eab1ddab0eafe2a4fe9313c77367a8467a88991efa1ce9f5b1a6b3c64543de4705bba02741ac55115bf1dc7e04c058478d8b8e85e125e397a9cfff74414480419ea5da061a68103cd37a0e6086",
                IsDisabled = true
            };

            // Act
            var serviceResult = await _costCentresAppService.CreateAsync(input);

            // Assert
            var result = await _costCentreRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.CostCentreReference.ShouldBe("7b2237251968459784bd");
            result.CostCentreName.ShouldBe("e558dfcea31d478992479dd4c2aa0f21ffa07fe1d16c48eab1ddab0eafe2a4fe9313c77367a8467a88991efa1ce9f5b1a6b3c64543de4705bba02741ac55115bf1dc7e04c058478d8b8e85e125e397a9cfff74414480419ea5da061a68103cd37a0e6086");
            result.IsDisabled.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new CostCentreUpdateDto()
            {
                CostCentreReference = "ec425eddab9e4eb7997c",
                CostCentreName = "f8e1d2becb784b3e846e81f37ee4892a049385046e9a49b6835901577098bea0e3414299268845d99a8c45e4e60b8c815b97ac3f63844a008ad0b050405f7618a016c47590774341be889b1b8407254b181a6edc31c6409cb47df58ef4bf069939bc3818",
                IsDisabled = true
            };

            // Act
            var serviceResult = await _costCentresAppService.UpdateAsync(Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"), input);

            // Assert
            var result = await _costCentreRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.CostCentreReference.ShouldBe("ec425eddab9e4eb7997c");
            result.CostCentreName.ShouldBe("f8e1d2becb784b3e846e81f37ee4892a049385046e9a49b6835901577098bea0e3414299268845d99a8c45e4e60b8c815b97ac3f63844a008ad0b050405f7618a016c47590774341be889b1b8407254b181a6edc31c6409cb47df58ef4bf069939bc3818");
            result.IsDisabled.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _costCentresAppService.DeleteAsync(Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"));

            // Assert
            var result = await _costCentreRepository.FindAsync(c => c.Id == Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"));

            result.ShouldBeNull();
        }
    }
}