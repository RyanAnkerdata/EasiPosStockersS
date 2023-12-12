using System;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreExcelDtoBase
    {
        public string CostCentreReference { get; set; } = null!;
        public string CostCentreName { get; set; } = null!;
        public bool IsDisabled { get; set; }
    }
}