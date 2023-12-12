using System;

namespace EasiPosStockers.Branches
{
    public abstract class BranchExcelDtoBase
    {
        public string BranchReference { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string TaxRegistrationNumber { get; set; } = null!;
    }
}