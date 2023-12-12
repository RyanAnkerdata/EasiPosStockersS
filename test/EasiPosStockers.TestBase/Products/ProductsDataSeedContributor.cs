using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using EasiPosStockers.Products;

namespace EasiPosStockers.Products
{
    public class ProductsDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProductsDataSeedContributor(IProductRepository productRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _productRepository = productRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _productRepository.InsertAsync(new Product
            (
                id: Guid.Parse("c36db44e-88f0-4425-8e7a-ebbfb288eb66"),
                description: "7e27f35708654018ba0b18c76b94ef17fa2df1ec1734417886715e358521f34c60662bb930924e30ac3095b10e2a879695060192785848859dc6c4965cb4857a3f1ab229b9e44e1b85ad2800788d96f5baaae6be22c14a5c93330fe23caef3d9e071b16e",
                productName: "43158edb3b674639a37e9088885f49e319e6ec57c28b4fe288cf7a197a801f792fe81f96857a4688b070a7811cc0ea08ecc0"
            ));

            await _productRepository.InsertAsync(new Product
            (
                id: Guid.Parse("7f7cef3c-f07e-47a7-9d12-d7569d4ff806"),
                description: "c01222825e0547f2a1dc1ca64b8ef439d193460778a4417d97dcc1546f37f2fcf15b2ae3d25f4115ac267f286f7632bbf3c78940cca3400a963bf91c71e4fa0ad468a003c2dd4807bce9c53f3f8aaa7dec959b496c4c44688baf322416cdea413c28bdde",
                productName: "0e45cfba36ee40ebbf6a1c778a051e65b325a2c64a1b45fe9b207347370dd413b2354e9494dd4b629f4c4af2670ebedc1f46"
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}