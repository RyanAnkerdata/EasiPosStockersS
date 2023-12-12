using EasiPosStockers.Branches;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreBase : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string CostCentreReference { get; set; }

        [NotNull]
        public virtual string CostCentreName { get; set; }

        public virtual bool IsDisabled { get; set; }
        public Guid? BranchId { get; set; }

        protected CostCentreBase()
        {

        }

        public CostCentreBase(Guid id, Guid? branchId, string costCentreReference, string costCentreName, bool isDisabled)
        {

            Id = id;
            Check.NotNull(costCentreReference, nameof(costCentreReference));
            Check.Length(costCentreReference, nameof(costCentreReference), CostCentreConsts.CostCentreReferenceMaxLength, 0);
            Check.NotNull(costCentreName, nameof(costCentreName));
            Check.Length(costCentreName, nameof(costCentreName), CostCentreConsts.CostCentreNameMaxLength, 0);
            CostCentreReference = costCentreReference;
            CostCentreName = costCentreName;
            IsDisabled = isDisabled;
            BranchId = branchId;
        }

    }
}