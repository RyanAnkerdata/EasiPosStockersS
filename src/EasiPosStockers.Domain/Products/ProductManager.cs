using EasiPosStockers.CostCentres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace EasiPosStockers.Products
{
    public abstract class ProductManagerBase : DomainService
    {
        protected IProductRepository _productRepository;
        protected IRepository<CostCentre, Guid> _costCentreRepository;

        public ProductManagerBase(IProductRepository productRepository,
        IRepository<CostCentre, Guid> costCentreRepository)
        {
            _productRepository = productRepository;
            _costCentreRepository = costCentreRepository;
        }

        public virtual async Task<Product> CreateAsync(
        List<Guid> costCentreIds,
        string description, string productName)
        {
            Check.NotNullOrWhiteSpace(description, nameof(description));
            Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength);
            Check.NotNullOrWhiteSpace(productName, nameof(productName));
            Check.Length(productName, nameof(productName), ProductConsts.ProductNameMaxLength);

            var product = new Product(
             GuidGenerator.Create(),
             description, productName
             );

            await SetCostCentresAsync(product, costCentreIds);

            return await _productRepository.InsertAsync(product);
        }

        public virtual async Task<Product> UpdateAsync(
            Guid id,
            List<Guid> costCentreIds,
        string description, string productName, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(description, nameof(description));
            Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength);
            Check.NotNullOrWhiteSpace(productName, nameof(productName));
            Check.Length(productName, nameof(productName), ProductConsts.ProductNameMaxLength);

            var queryable = await _productRepository.WithDetailsAsync(x => x.CostCentres);
            var query = queryable.Where(x => x.Id == id);

            var product = await AsyncExecuter.FirstOrDefaultAsync(query);

            product.Description = description;
            product.ProductName = productName;

            await SetCostCentresAsync(product, costCentreIds);

            product.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _productRepository.UpdateAsync(product);
        }

        private async Task SetCostCentresAsync(Product product, List<Guid> costCentreIds)
        {
            if (costCentreIds == null || !costCentreIds.Any())
            {
                product.RemoveAllCostCentres();
                return;
            }

            var query = (await _costCentreRepository.GetQueryableAsync())
                .Where(x => costCentreIds.Contains(x.Id))
                .Select(x => x.Id);

            var costCentreIdsInDb = await AsyncExecuter.ToListAsync(query);
            if (!costCentreIdsInDb.Any())
            {
                return;
            }

            product.RemoveAllCostCentresExceptGivenIds(costCentreIdsInDb);

            foreach (var costCentreId in costCentreIdsInDb)
            {
                product.AddCostCentre(costCentreId);
            }
        }

    }
}