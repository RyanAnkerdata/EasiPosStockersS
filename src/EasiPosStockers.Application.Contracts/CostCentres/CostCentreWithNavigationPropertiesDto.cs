using EasiPosStockers.Branches;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreWithNavigationPropertiesDtoBase
    {
        public CostCentreDto CostCentre { get; set; } = null!;

        public BranchDto Branch { get; set; } = null!;

    }
}