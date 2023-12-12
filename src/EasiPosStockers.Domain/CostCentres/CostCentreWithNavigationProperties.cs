using EasiPosStockers.Branches;

using System;
using System.Collections.Generic;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreWithNavigationPropertiesBase
    {
        public CostCentre CostCentre { get; set; } = null!;

        public Branch Branch { get; set; } = null!;
        

        
    }
}