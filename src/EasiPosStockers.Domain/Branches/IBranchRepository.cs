using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasiPosStockers.Branches
{
    public partial interface IBranchRepository : IRepository<Branch, Guid>
    {
        Task<List<Branch>> GetListAsync(
            string? filterText = null,
            string? branchReference = null,
            string? branchName = null,
            string? taxRegistrationNumber = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? branchReference = null,
            string? branchName = null,
            string? taxRegistrationNumber = null,
            CancellationToken cancellationToken = default);
    }
}