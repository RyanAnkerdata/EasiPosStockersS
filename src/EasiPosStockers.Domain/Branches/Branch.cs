using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace EasiPosStockers.Branches
{
    public abstract class BranchBase : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string BranchReference { get; set; }

        [NotNull]
        public virtual string BranchName { get; set; }

        [NotNull]
        public virtual string TaxRegistrationNumber { get; set; }

        protected BranchBase()
        {

        }

        public BranchBase(Guid id, string branchReference, string branchName, string taxRegistrationNumber)
        {

            Id = id;
            Check.NotNull(branchReference, nameof(branchReference));
            Check.Length(branchReference, nameof(branchReference), BranchConsts.BranchReferenceMaxLength, 0);
            Check.NotNull(branchName, nameof(branchName));
            Check.Length(branchName, nameof(branchName), BranchConsts.BranchNameMaxLength, 0);
            Check.NotNull(taxRegistrationNumber, nameof(taxRegistrationNumber));
            Check.Length(taxRegistrationNumber, nameof(taxRegistrationNumber), BranchConsts.TaxRegistrationNumberMaxLength, 0);
            BranchReference = branchReference;
            BranchName = branchName;
            TaxRegistrationNumber = taxRegistrationNumber;
        }

    }
}