using Volo.Abp.Application.Dtos;
using System;

namespace EasiPosStockers.Branches
{
    public abstract class BranchExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? BranchReference { get; set; }
        public string? BranchName { get; set; }
        public string? TaxRegistrationNumber { get; set; }

        public BranchExcelDownloadDtoBase()
        {

        }
    }
}