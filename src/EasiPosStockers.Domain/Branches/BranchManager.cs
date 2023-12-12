using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace EasiPosStockers.Branches
{
    public abstract class BranchManagerBase : DomainService
    {
        protected IBranchRepository _branchRepository;

        public BranchManagerBase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public virtual async Task<Branch> CreateAsync(
        string branchReference, string branchName, string taxRegistrationNumber)
        {
            Check.NotNullOrWhiteSpace(branchReference, nameof(branchReference));
            Check.Length(branchReference, nameof(branchReference), BranchConsts.BranchReferenceMaxLength);
            Check.NotNullOrWhiteSpace(branchName, nameof(branchName));
            Check.Length(branchName, nameof(branchName), BranchConsts.BranchNameMaxLength);
            Check.NotNullOrWhiteSpace(taxRegistrationNumber, nameof(taxRegistrationNumber));
            Check.Length(taxRegistrationNumber, nameof(taxRegistrationNumber), BranchConsts.TaxRegistrationNumberMaxLength);

            var branch = new Branch(
             GuidGenerator.Create(),
             branchReference, branchName, taxRegistrationNumber
             );

            return await _branchRepository.InsertAsync(branch);
        }

        public virtual async Task<Branch> UpdateAsync(
            Guid id,
            string branchReference, string branchName, string taxRegistrationNumber, [CanBeNull] string? concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(branchReference, nameof(branchReference));
            Check.Length(branchReference, nameof(branchReference), BranchConsts.BranchReferenceMaxLength);
            Check.NotNullOrWhiteSpace(branchName, nameof(branchName));
            Check.Length(branchName, nameof(branchName), BranchConsts.BranchNameMaxLength);
            Check.NotNullOrWhiteSpace(taxRegistrationNumber, nameof(taxRegistrationNumber));
            Check.Length(taxRegistrationNumber, nameof(taxRegistrationNumber), BranchConsts.TaxRegistrationNumberMaxLength);

            var branch = await _branchRepository.GetAsync(id);

            branch.BranchReference = branchReference;
            branch.BranchName = branchName;
            branch.TaxRegistrationNumber = taxRegistrationNumber;

            branch.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _branchRepository.UpdateAsync(branch);
        }

    }
}