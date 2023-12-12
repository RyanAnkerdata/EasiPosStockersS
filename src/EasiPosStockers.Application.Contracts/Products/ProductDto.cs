using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.Products
{
    public abstract class ProductDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Description { get; set; } = null!;
        public string ProductName { get; set; } = null!;

        public string ConcurrencyStamp { get; set; } = null!;

    }
}