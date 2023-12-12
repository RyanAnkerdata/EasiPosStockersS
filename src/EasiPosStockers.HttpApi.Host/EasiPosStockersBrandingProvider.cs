using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EasiPosStockers;

[Dependency(ReplaceServices = true)]
public class EasiPosStockersBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "EasiPosStockers";
}
