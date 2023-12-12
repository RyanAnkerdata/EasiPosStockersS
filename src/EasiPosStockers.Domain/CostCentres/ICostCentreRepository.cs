using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EasiPosStockers.CostCentres
{
    public partial interface ICostCentreRepository : IRepository<CostCentre, Guid>
    {
        Task<CostCentreWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<CostCentreWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null,
            Guid? branchId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<CostCentre>> GetListAsync(
                    string? filterText = null,
                    string? costCentreReference = null,
                    string? costCentreName = null,
                    bool? isDisabled = null,
                    string? sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string? filterText = null,
            string? costCentreReference = null,
            string? costCentreName = null,
            bool? isDisabled = null,
            Guid? branchId = null,
            CancellationToken cancellationToken = default);
    }
}