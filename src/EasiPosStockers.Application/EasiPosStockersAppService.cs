using EasiPosStockers.Localization;
using Volo.Abp.Application.Services;

namespace EasiPosStockers;

/* Inherit your application services from this class.
 */
public abstract class EasiPosStockersAppService : ApplicationService
{
    protected EasiPosStockersAppService()
    {
        LocalizationResource = typeof(EasiPosStockersResource);
    }
}
