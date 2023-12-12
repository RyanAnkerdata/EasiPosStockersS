using EasiPosStockers.Products;
using EasiPosStockers.CostCentres;
using Volo.Abp.AutoMapper;
using EasiPosStockers.Branches;
using AutoMapper;

namespace EasiPosStockers.Blazor;

public class EasiPosStockersBlazorAutoMapperProfile : Profile
{
    public EasiPosStockersBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.

        CreateMap<BranchDto, BranchUpdateDto>();

        CreateMap<CostCentreDto, CostCentreUpdateDto>();

        CreateMap<ProductDto, ProductUpdateDto>();

        //CreateMap<ProductDto, ProductUpdateDto>().Ignore(x => x.CostCentreIds);

        CreateMap<ProductDto, ProductUpdateDto>().Ignore(x => x.CostCentreIds);
    }
}