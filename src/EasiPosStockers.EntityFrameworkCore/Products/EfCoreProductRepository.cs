using EasiPosStockers.CostCentres;
using EasiPosStockers.CostCentres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using EasiPosStockers.EntityFrameworkCore;

namespace EasiPosStockers.Products
{
    public abstract class EfCoreProductRepositoryBase : EfCoreRepository<EasiPosStockersDbContext, Product, Guid>
    {
        public EfCoreProductRepositoryBase(IDbContextProvider<EasiPosStockersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<ProductWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id).Include(x => x.CostCentres)
                .Select(product => new ProductWithNavigationProperties
                {
                    Product = product,
                    CostCentres = (from productCostCentres in product.CostCentres
                                   join _costCentre in dbContext.Set<CostCentre>() on productCostCentres.CostCentreId equals _costCentre.Id
                                   select _costCentre).ToList()
                }).FirstOrDefault();
        }

        public virtual async Task<List<ProductWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? description = null,
            string? productName = null,
            Guid? costCentreId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, description, productName, costCentreId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProductConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<ProductWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from product in (await GetDbSetAsync())

                   select new ProductWithNavigationProperties
                   {
                       Product = product,
                       CostCentres = new List<CostCentre>()
                   };
        }

        protected virtual IQueryable<ProductWithNavigationProperties> ApplyFilter(
            IQueryable<ProductWithNavigationProperties> query,
            string? filterText,
            string? description = null,
            string? productName = null,
            Guid? costCentreId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Product.Description!.Contains(filterText!) || e.Product.ProductName!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Product.Description.Contains(description))
                    .WhereIf(!string.IsNullOrWhiteSpace(productName), e => e.Product.ProductName.Contains(productName))
                    .WhereIf(costCentreId != null && costCentreId != Guid.Empty, e => e.Product.CostCentres.Any(x => x.CostCentreId == costCentreId));
        }

        public virtual async Task<List<Product>> GetListAsync(
            string? filterText = null,
            string? description = null,
            string? productName = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, description, productName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? ProductConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? description = null,
            string? productName = null,
            Guid? costCentreId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, description, productName, costCentreId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Product> ApplyFilter(
            IQueryable<Product> query,
            string? filterText = null,
            string? description = null,
            string? productName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Description!.Contains(filterText!) || e.ProductName!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(description), e => e.Description.Contains(description))
                    .WhereIf(!string.IsNullOrWhiteSpace(productName), e => e.ProductName.Contains(productName));
        }
    }
}