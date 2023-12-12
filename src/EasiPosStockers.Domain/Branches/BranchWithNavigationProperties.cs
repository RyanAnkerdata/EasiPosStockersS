using EasiPosStockers.CostCentres;

using System;
using System.Collections.Generic;

namespace EasiPosStockers.Branches
{
    public abstract class BranchWithNavigationPropertiesBase
    {
        public Branch Branch { get; set; } = null!;

        public CostCentre CostCentre { get; set; } = null!;
        

        
    }
}