using EasiPosStockers.CostCentres;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace EasiPosStockers.Branches
{
    public abstract class BranchWithNavigationPropertiesDtoBase
    {
        public BranchDto Branch { get; set; } = null!;

        public CostCentreDto CostCentre { get; set; } = null!;

    }
}