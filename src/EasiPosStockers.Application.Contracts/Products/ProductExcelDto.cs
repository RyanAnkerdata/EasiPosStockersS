using System;

namespace EasiPosStockers.Products
{
    public abstract class ProductExcelDtoBase
    {
        public string Description { get; set; } = null!;
        public string ProductName { get; set; } = null!;
    }
}