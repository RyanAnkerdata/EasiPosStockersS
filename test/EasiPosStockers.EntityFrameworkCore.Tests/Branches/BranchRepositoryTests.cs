using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using EasiPosStockers.Branches;
using EasiPosStockers.EntityFrameworkCore;
using Xunit;

namespace EasiPosStockers.EntityFrameworkCore.Domains.Branches
{
    public class BranchRepositoryTests : EasiPosStockersEntityFrameworkCoreTestBase
    {
        private readonly IBranchRepository _branchRepository;

        public BranchRepositoryTests()
        {
            _branchRepository = GetRequiredService<IBranchRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _branchRepository.GetListAsync(
                    branchReference: "0228d013590d4ccd8691",
                    branchName: "268dd21ba233440bbec1",
                    taxRegistrationNumber: "a2a2d457b95c47c7957f"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _branchRepository.GetCountAsync(
                    branchReference: "47e118c168244fe5a801",
                    branchName: "0b53b739686f41bb9657",
                    taxRegistrationNumber: "9c61adba084e453a8a43"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}