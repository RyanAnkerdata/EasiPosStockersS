using System;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.Products
{
    public class ProductCostCentre : Entity
    {

        public Guid ProductId { get; protected set; }

        public Guid CostCentreId { get; protected set; }

        private ProductCostCentre()
        {

        }

        public ProductCostCentre(Guid productId, Guid costCentreId)
        {
            ProductId = productId;
            CostCentreId = costCentreId;
        }

        public override object[] GetKeys()
        {
            return new object[]
                {
                    ProductId,
                    CostCentreId
                };
        }
    }
}