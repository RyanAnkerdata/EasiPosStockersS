using EasiPosStockers.CostCentres;
using EasiPosStockers.CostCentres;
using System;
using Volo.Abp.Domain.Services;
using Volo.Abp.Domain.Repositories;

namespace EasiPosStockers.Products
{
    public class ProductManager : ProductManagerBase
    {
        //<suite-custom-code-autogenerated>
        public ProductManager(IProductRepository productRepository,
        IRepository<CostCentre, Guid> costCentreRepository)
            : base(productRepository, costCentreRepository)
        {
        }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}