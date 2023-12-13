using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.Branches
{

   
    public abstract class BranchDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {


        public string BranchReference { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string TaxRegistrationNumber { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;

    }
}