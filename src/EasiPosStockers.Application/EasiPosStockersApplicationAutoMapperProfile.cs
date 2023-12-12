using EasiPosStockers.Products;
using EasiPosStockers.CostCentres;
using System;
using EasiPosStockers.Shared;
using Volo.Abp.AutoMapper;
using EasiPosStockers.Branches;
using AutoMapper;

namespace EasiPosStockers;

public class EasiPosStockersApplicationAutoMapperProfile : Profile
{
    public EasiPosStockersApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Branch, BranchDto>();
        CreateMap<Branch, BranchExcelDto>();

        CreateMap<CostCentre, CostCentreDto>();
        CreateMap<CostCentre, CostCentreExcelDto>();

        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductExcelDto>();

        CreateMap<CostCentreWithNavigationProperties, CostCentreWithNavigationPropertiesDto>();
        CreateMap<Product, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.ProductName));

        CreateMap<BranchWithNavigationProperties, BranchWithNavigationPropertiesDto>();
        CreateMap<CostCentre, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.CostCentreName));

        CreateMap<Branch, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.BranchName));

        CreateMap<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>();
    }
}