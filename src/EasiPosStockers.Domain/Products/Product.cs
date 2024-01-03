using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace EasiPosStockers.Products
{
    public abstract class ProductBase : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [NotNull]
        public virtual string Description { get; set; }

        [NotNull]
        public virtual string ProductName { get; set; }

        public ICollection<ProductCostCentre> CostCentres { get; private set; }

        protected ProductBase()
        {

        }

        public ProductBase(Guid id, string description, string productName)
        {

            Id = id;
            Check.NotNull(description, nameof(description));
            Check.Length(description, nameof(description), ProductConsts.DescriptionMaxLength, 0);
            Check.NotNull(productName, nameof(productName));
            Check.Length(productName, nameof(productName), ProductConsts.ProductNameMaxLength, 0);
            Description = description;
            ProductName = productName;
            CostCentres = new Collection<ProductCostCentre>();
        }
        public virtual void AddCostCentre(Guid costCentreId)
        {
            Check.NotNull(costCentreId, nameof(costCentreId));

            if (IsInCostCentres(costCentreId))
            {
                return;
            }

            CostCentres.Add(new ProductCostCentre(Id, costCentreId)); // THIS IS KEY!!
        }

        public virtual void RemoveCostCentre(Guid costCentreId)
        {
            Check.NotNull(costCentreId, nameof(costCentreId));

            if (!IsInCostCentres(costCentreId))
            {
                return;
            }

            CostCentres.RemoveAll(x => x.CostCentreId == costCentreId);
        }

        public virtual void RemoveAllCostCentresExceptGivenIds(List<Guid> costCentreIds)
        {
            Check.NotNullOrEmpty(costCentreIds, nameof(costCentreIds));

            CostCentres.RemoveAll(x => !costCentreIds.Contains(x.CostCentreId));
        }

        public virtual void RemoveAllCostCentres()
        {
            CostCentres.RemoveAll(x => x.ProductId == Id);
        }

        private bool IsInCostCentres(Guid costCentreId)
        {
            return CostCentres.Any(x => x.CostCentreId == costCentreId);
        }
    }
}