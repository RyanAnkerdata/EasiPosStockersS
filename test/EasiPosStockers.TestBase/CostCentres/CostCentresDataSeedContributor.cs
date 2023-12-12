using EasiPosStockers.Branches;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using EasiPosStockers.CostCentres;

namespace EasiPosStockers.CostCentres
{
    public class CostCentresDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly ICostCentreRepository _costCentreRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly BranchesDataSeedContributor _branchesDataSeedContributor;

        public CostCentresDataSeedContributor(ICostCentreRepository costCentreRepository, IUnitOfWorkManager unitOfWorkManager, BranchesDataSeedContributor branchesDataSeedContributor)
        {
            _costCentreRepository = costCentreRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _branchesDataSeedContributor = branchesDataSeedContributor;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _branchesDataSeedContributor.SeedAsync(context);

            await _costCentreRepository.InsertAsync(new CostCentre
            (
                id: Guid.Parse("d1c4a967-37e9-4dba-8917-76e153f66d2b"),
                costCentreReference: "a8d2d8334e1248348f21",
                costCentreName: "e479c0279f73445a8ddab5be185409d6db805ac0f37f47c8852d20527ff8ec277cfbf140840346ff9e1932997a9a6b0698e0756d025f49058946157f278da540e37427dc1c49420aab62b107af015d79b748dc19c6644ba7a26379b16e7c86c382443d71",
                isDisabled: true,
                branchId: null
            ));

            await _costCentreRepository.InsertAsync(new CostCentre
            (
                id: Guid.Parse("99f9495b-6f6d-4a61-bd64-cc3c63086af0"),
                costCentreReference: "4ba27d4e24bb497aa995",
                costCentreName: "8512bb1185b44447b9c8a4d4e9342932a6df7e752faf48dc9fcde8b877259fe66f3aab33a0a041e1a141b433a6ff33fd56906bddfdf142eea31e6f315154a9052a54d23586d4415d8364e16d862e625710f3c7829aed478faceaecc69454d1e1c3309d88",
                isDisabled: true,
                branchId: null
            ));

            await _unitOfWorkManager!.Current!.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}