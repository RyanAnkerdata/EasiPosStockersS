using EasiPosStockers.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EasiPosStockers.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EasiPosStockersController : AbpControllerBase
{
    protected EasiPosStockersController()
    {
        LocalizationResource = typeof(EasiPosStockersResource);
    }
}
