using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using EasiPosStockers.EntityFrameworkCore;

namespace EasiPosStockers.CostCentres
{
    public class EfCoreCostCentreRepository : EfCoreCostCentreRepositoryBase, ICostCentreRepository
    {
        public EfCoreCostCentreRepository(IDbContextProvider<EasiPosStockersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}