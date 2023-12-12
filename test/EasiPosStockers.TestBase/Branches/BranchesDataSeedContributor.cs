using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using EasiPosStockers.Branches;

namespace EasiPosStockers.Branches
{
    public class BranchesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IBranchRepository _branchRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public BranchesDataSeedContributor(IBranchRepository branchRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _branchRepository = branchRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _branchRepository.InsertAsync(new Branch
            (
                id: Guid.Parse("16d65d1a-2ea9-487e-9e0d-42b871b517b9"),
                branchReference: "0228d013590d4ccd8691",
                branchName: "268dd21ba233440bbec1",
                taxRegistrationNumber: "a2a2d457b95c47c7957f"
            ));

            await _branchRepository.InsertAsync(new Branch
            (
                id: Guid.Parse("de0062a7-9012-4327-8ce0-702caa2181c7"),
                branchReference: "47e118c168244fe5a801",
                branchName: "0b53b739686f41bb9657",
                taxRegistrationNumber: "9c61adba084e453a8a43"
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}