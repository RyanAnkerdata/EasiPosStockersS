using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace EasiPosStockers.Products
{
    public abstract class ProductsAppServiceTests<TStartupModule> : EasiPosStockersApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductsAppServiceTests()
        {
            _productsAppService = GetRequiredService<IProductsAppService>();
            _productRepository = GetRequiredService<IRepository<Product, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _productsAppService.GetListAsync(new GetProductsInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Product.Id == Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66")).ShouldBe(true);
            result.Items.Any(x => x.Product.Id == Guid.Parse("7f7cef3c-f07e-47a7-9d12-d7569d4ff806")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _productsAppService.GetAsync(Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ProductCreateDto
            {
                Description = "304ab6073878499c99150795d74034bf7484c1a1fa924b119ad6c14e22fd04a96de67f9224f24985a7ff4727449af0f118007b725d484d38947120e978d09725b306727a36ae4821ad827800a07a9f2832f5e5c465ba498cb10dd8f092d82ca8a2b4b3fc",
                ProductName = "b5d7e4ca247044b196c5d6d6f209ea3dcc9d033f553d4e55a4371c1964cd11c9f2a41ae233bf4280ae6896be24c4c0e4976d"
            };

            // Act
            var serviceResult = await _productsAppService.CreateAsync(input);

            // Assert
            var result = await _productRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Description.ShouldBe("304ab6073878499c99150795d74034bf7484c1a1fa924b119ad6c14e22fd04a96de67f9224f24985a7ff4727449af0f118007b725d484d38947120e978d09725b306727a36ae4821ad827800a07a9f2832f5e5c465ba498cb10dd8f092d82ca8a2b4b3fc");
            result.ProductName.ShouldBe("b5d7e4ca247044b196c5d6d6f209ea3dcc9d033f553d4e55a4371c1964cd11c9f2a41ae233bf4280ae6896be24c4c0e4976d");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ProductUpdateDto()
            {
                Description = "2181fec9f1074f86905b754d522118b29b3b39ee26c64a92928fbd0cdad1a31087e280aee7044550965276546f1638a3c86620cb312c4153bf56c1b011b71f74f6e4b4218e1540ed9239722f66d9f85d8e014e63c2aa4d91b343881564074b47cfe10121",
                ProductName = "5e88e555a84840f3a82e293f8e4bdbe91f0b816a74f4478482fb47c62b2bf157e119b6dbb6e843e1b103b1e938b50605170a"
            };

            // Act
            var serviceResult = await _productsAppService.UpdateAsync(Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66"), input);

            // Assert
            var result = await _productRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Description.ShouldBe("2181fec9f1074f86905b754d522118b29b3b39ee26c64a92928fbd0cdad1a31087e280aee7044550965276546f1638a3c86620cb312c4153bf56c1b011b71f74f6e4b4218e1540ed9239722f66d9f85d8e014e63c2aa4d91b343881564074b47cfe10121");
            result.ProductName.ShouldBe("5e88e555a84840f3a82e293f8e4bdbe91f0b816a74f4478482fb47c62b2bf157e119b6dbb6e843e1b103b1e938b50605170a");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _productsAppService.DeleteAsync(Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66"));

            // Assert
            var result = await _productRepository.FindAsync(c => c.Id == Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66"));

            result.ShouldBeNull();
        }
    }
}