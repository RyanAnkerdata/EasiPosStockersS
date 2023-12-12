using Volo.Abp.Application.Dtos;
using System;

namespace EasiPosStockers.Products
{
    public abstract class GetProductsInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? Description { get; set; }
        public string? ProductName { get; set; }
        public Guid? CostCentreId { get; set; }

        public GetProductsInputBase()
        {

        }
    }
}