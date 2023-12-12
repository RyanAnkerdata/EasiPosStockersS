using EasiPosStockers.Branches;
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

namespace EasiPosStockers.CostCentres
{
    public abstract class EfCoreCostCentreRepositoryBase : EfCoreRepository<EasiPosStockersDbContext, CostCentre, Guid>
    {
        public EfCoreCostCentreRepositoryBase(IDbContextProvider<EasiPosStockersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<CostCentreWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(costCentre => new CostCentreWithNavigationProperties
                {
                    CostCentre = costCentre,
                    Branch = dbContext.Set<Branch>().FirstOrDefault(c => c.Id == costCentre.BranchId)
                }).FirstOrDefault();
        }

        public virtual async Task<List<CostCentreWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null,
            Guid? branchId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, costCentreReference, costCentreName, isDisabled, branchId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CostCentreConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<CostCentreWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from costCentre in (await GetDbSetAsync())
                   join branch in (await GetDbContextAsync()).Set<Branch>() on costCentre.BranchId equals branch.Id into branches
                   from branch in branches.DefaultIfEmpty()
                   select new CostCentreWithNavigationProperties
                   {
                       CostCentre = costCentre,
                       Branch = branch
                   };
        }

        protected virtual IQueryable<CostCentreWithNavigationProperties> ApplyFilter(
            IQueryable<CostCentreWithNavigationProperties> query,
            string? filterText,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null,
            Guid? branchId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.CostCentre.CostCentreReference!.Contains(filterText!) || e.CostCentre.CostCentreName!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(costCentreReference), e => e.CostCentre.CostCentreReference.Contains(costCentreReference))
                    .WhereIf(!string.IsNullOrWhiteSpace(costCentreName), e => e.CostCentre.CostCentreName.Contains(costCentreName))
                    .WhereIf(isDisabled.HasValue, e => e.CostCentre.IsDisabled == isDisabled)
                    .WhereIf(branchId != null && branchId != Guid.Empty, e => e.Branch != null && e.Branch.Id == branchId);
        }

        public virtual async Task<List<CostCentre>> GetListAsync(
            string? filterText = null,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, costCentreReference, costCentreName, isDisabled);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? CostCentreConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null,
            Guid? branchId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, costCentreReference, costCentreName, isDisabled, branchId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<CostCentre> ApplyFilter(
            IQueryable<CostCentre> query,
            string? filterText = null,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.CostCentreReference!.Contains(filterText!) || e.CostCentreName!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(costCentreReference), e => e.CostCentreReference.Contains(costCentreReference))
                    .WhereIf(!string.IsNullOrWhiteSpace(costCentreName), e => e.CostCentreName.Contains(costCentreName))
                    .WhereIf(isDisabled.HasValue, e => e.IsDisabled == isDisabled);
        }
    }
}