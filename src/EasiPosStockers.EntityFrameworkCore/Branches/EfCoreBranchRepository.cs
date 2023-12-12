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

namespace EasiPosStockers.Branches
{
    public abstract class EfCoreBranchRepositoryBase : EfCoreRepository<EasiPosStockersDbContext, Branch, Guid>
    {
        public EfCoreBranchRepositoryBase(IDbContextProvider<EasiPosStockersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<List<Branch>> GetListAsync(
            string? filterText = null,
            string? branchReference = null,
            string? branchName = null,
            string? taxRegistrationNumber = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, branchReference, branchName, taxRegistrationNumber);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? BranchConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public virtual async Task<long> GetCountAsync(
            string? filterText = null,
            string? branchReference = null,
            string? branchName = null,
            string? taxRegistrationNumber = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, branchReference, branchName, taxRegistrationNumber);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Branch> ApplyFilter(
            IQueryable<Branch> query,
            string? filterText = null,
            string? branchReference = null,
            string? branchName = null,
            string? taxRegistrationNumber = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.BranchReference!.Contains(filterText!) || e.BranchName!.Contains(filterText!) || e.TaxRegistrationNumber!.Contains(filterText!))
                    .WhereIf(!string.IsNullOrWhiteSpace(branchReference), e => e.BranchReference.Contains(branchReference))
                    .WhereIf(!string.IsNullOrWhiteSpace(branchName), e => e.BranchName.Contains(branchName))
                    .WhereIf(!string.IsNullOrWhiteSpace(taxRegistrationNumber), e => e.TaxRegistrationNumber.Contains(taxRegistrationNumber));
        }
    }
}