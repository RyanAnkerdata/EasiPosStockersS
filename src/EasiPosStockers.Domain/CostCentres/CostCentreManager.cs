using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreManagerBase : DomainService
    {
        protected ICostCentreRepository _costCentreRepository;

        public CostCentreManagerBase(ICostCentreRepository costCentreRepository)
        {
            _costCentreRepository = costCentreRepository;
        }

        public virtual async Task<CostCentre> CreateAsync(
        Guid? branchId, string costCentreReference, string costCentreName, bool isDisabled)
        {
            Check.NotNullOrWhiteSpace(costCentreReference, nameof(costCentreReference));
            Check.Length(costCentreReference, nameof(costCentreReference), CostCentreConsts.CostCentreReferenceMaxLength);
            Check.NotNullOrWhiteSpace(costCentreName, nameof(costCentreName));
            Check.Length(costCentreName, nameof(costCentreName), CostCentreConsts.CostCentreNameMaxLength);

            var costCentre = new CostCentre(
             GuidGenerator.Create(),
             branchId, costCentreReference, costCentreName, isDisabled
             );

            return await _costCentreRepository.InsertAsync(costCentre);
        }

        public virtual async Task<CostCentre> UpdateAsync(
            Guid id,
            Guid? branchId, string costCentreReference, string costCentreName, bool isDisabled, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(costCentreReference, nameof(costCentreReference));
            Check.Length(costCentreReference, nameof(costCentreReference), CostCentreConsts.CostCentreReferenceMaxLength);
            Check.NotNullOrWhiteSpace(costCentreName, nameof(costCentreName));
            Check.Length(costCentreName, nameof(costCentreName), CostCentreConsts.CostCentreNameMaxLength);

            var costCentre = await _costCentreRepository.GetAsync(id);

            costCentre.BranchId = branchId;
            costCentre.CostCentreReference = costCentreReference;
            costCentre.CostCentreName = costCentreName;
            costCentre.IsDisabled = isDisabled;

            costCentre.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _costCentreRepository.UpdateAsync(costCentre);
        }

    }
}