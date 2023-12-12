using EasiPosStockers.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace EasiPosStockers.Permissions;

public class EasiPosStockersPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EasiPosStockersPermissions.GroupName);

        myGroup.AddPermission(EasiPosStockersPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(EasiPosStockersPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(EasiPosStockersPermissions.MyPermission1, L("Permission:MyPermission1"));

        var branchPermission = myGroup.AddPermission(EasiPosStockersPermissions.Branches.Default, L("Permission:Branches"));
        branchPermission.AddChild(EasiPosStockersPermissions.Branches.Create, L("Permission:Create"));
        branchPermission.AddChild(EasiPosStockersPermissions.Branches.Edit, L("Permission:Edit"));
        branchPermission.AddChild(EasiPosStockersPermissions.Branches.Delete, L("Permission:Delete"));

        var costCentrePermission = myGroup.AddPermission(EasiPosStockersPermissions.CostCentres.Default, L("Permission:CostCentres"));
        costCentrePermission.AddChild(EasiPosStockersPermissions.CostCentres.Create, L("Permission:Create"));
        costCentrePermission.AddChild(EasiPosStockersPermissions.CostCentres.Edit, L("Permission:Edit"));
        costCentrePermission.AddChild(EasiPosStockersPermissions.CostCentres.Delete, L("Permission:Delete"));

        var productPermission = myGroup.AddPermission(EasiPosStockersPermissions.Products.Default, L("Permission:Products"));
        productPermission.AddChild(EasiPosStockersPermissions.Products.Create, L("Permission:Create"));
        productPermission.AddChild(EasiPosStockersPermissions.Products.Edit, L("Permission:Edit"));
        productPermission.AddChild(EasiPosStockersPermissions.Products.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EasiPosStockersResource>(name);
    }
}