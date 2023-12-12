using EasiPosStockers.Localization;
using Volo.Abp.AspNetCore.Components;

namespace EasiPosStockers.Blazor;

public abstract class EasiPosStockersComponentBase : AbpComponentBase
{
    protected EasiPosStockersComponentBase()
    {
        LocalizationResource = typeof(EasiPosStockersResource);
    }
}
