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

namespace EasiPosStockers.Branches
{
    public class EfCoreBranchRepository : EfCoreBranchRepositoryBase, IBranchRepository
    {
        public EfCoreBranchRepository(IDbContextProvider<EasiPosStockersDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}