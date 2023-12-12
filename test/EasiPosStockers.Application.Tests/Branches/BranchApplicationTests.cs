using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace EasiPosStockers.Branches
{
    public abstract class BranchesAppServiceTests<TStartupModule> : EasiPosStockersApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IBranchesAppService _branchesAppService;
        private readonly IRepository<Branch, Guid> _branchRepository;

        public BranchesAppServiceTests()
        {
            _branchesAppService = GetRequiredService<IBranchesAppService>();
            _branchRepository = GetRequiredService<IRepository<Branch, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _branchesAppService.GetListAsync(new GetBranchesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("de0062a7-9012-4327-8ce0-702caa2181c7")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _branchesAppService.GetAsync(Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new BranchCreateDto
            {
                BranchReference = "dfe10250ea0d4e809d7d",
                BranchName = "d22ef254f420429b8019",
                TaxRegistrationNumber = "616b9705370c44f4a310"
            };

            // Act
            var serviceResult = await _branchesAppService.CreateAsync(input);

            // Assert
            var result = await _branchRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.BranchReference.ShouldBe("dfe10250ea0d4e809d7d");
            result.BranchName.ShouldBe("d22ef254f420429b8019");
            result.TaxRegistrationNumber.ShouldBe("616b9705370c44f4a310");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new BranchUpdateDto()
            {
                BranchReference = "5cc7872804dc4b0faae5",
                BranchName = "fdc7758269cd41e8bf90",
                TaxRegistrationNumber = "999845747f424641a6cc"
            };

            // Act
            var serviceResult = await _branchesAppService.UpdateAsync(Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"), input);

            // Assert
            var result = await _branchRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.BranchReference.ShouldBe("5cc7872804dc4b0faae5");
            result.BranchName.ShouldBe("fdc7758269cd41e8bf90");
            result.TaxRegistrationNumber.ShouldBe("999845747f424641a6cc");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _branchesAppService.DeleteAsync(Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"));

            // Assert
            var result = await _branchRepository.FindAsync(c => c.Id == Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"));

            result.ShouldBeNull();
        }
    }
}