using System;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.CostCentres
{
    public class CostCentreBranch : Entity
    {

        public Guid CostCentreId { get; protected set; }

        public Guid BranchId { get; protected set; }

        private CostCentreBranch()
        {

        }

        public CostCentreBranch(Guid costCentreId, Guid branchId)
        {
            CostCentreId = costCentreId;
            BranchId = branchId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    CostCentreId,
                    BranchId
                };
        }
    }
}