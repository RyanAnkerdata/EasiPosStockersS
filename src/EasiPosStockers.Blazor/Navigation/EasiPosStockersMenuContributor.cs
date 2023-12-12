using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using EasiPosStockers.Localization;
using EasiPosStockers.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.AuditLogging.Blazor.Menus;
using Volo.Abp.Identity.Pro.Blazor.Navigation;
using Volo.Abp.LanguageManagement.Blazor.Menus;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TextTemplateManagement.Blazor.Menus;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.OpenIddict.Pro.Blazor.Menus;
using Volo.Saas.Host.Blazor.Navigation;

namespace EasiPosStockers.Blazor.Navigation;

public class EasiPosStockersMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public EasiPosStockersMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<EasiPosStockersResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
            EasiPosStockersMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 1
        ));

        //HostDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                EasiPosStockersMenus.HostDashboard,
                l["Menu:Dashboard"],
                "/HostDashboard",
                icon: "fa fa-chart-line",
                order: 2
            ).RequirePermissions(EasiPosStockersPermissions.Dashboard.Host)
        );

        //TenantDashboard
        context.Menu.AddItem(
            new ApplicationMenuItem(
                EasiPosStockersMenus.TenantDashboard,
                l["Menu:Dashboard"],
                "/Dashboard",
                icon: "fa fa-chart-line",
                order: 2
            ).RequirePermissions(EasiPosStockersPermissions.Dashboard.Tenant)
        );




        context.Menu.AddItem(
    new ApplicationMenuItem("Menu0", "Branches")
        .AddItem(
            new ApplicationMenuItem("Menu0.1", "Branch 1", url: "/test021")
                .AddItem(new ApplicationMenuItem("Menu0.1.1", "Cost Center 1", url: "/test021"))
                .AddItem(new ApplicationMenuItem("Menu0.1.2", "Cost Center 2", url: "/test021"))
        )
        .AddItem(
            new ApplicationMenuItem("Menu0.2", "Branch 2", url: "/test021")
                .AddItem(new ApplicationMenuItem("Menu0.2.1", "Cost Center 1", url: "/test021"))
                .AddItem(new ApplicationMenuItem("Menu0.2.2", "Cost Center 2", url: "/test021"))
        )
        .AddItem(
            new ApplicationMenuItem("Menu0.3", "Branch 3", url: "/test021")
                .AddItem(new ApplicationMenuItem("Menu0.3.1", "Cost Center 1", url: "/test021"))
                .AddItem(new ApplicationMenuItem("Menu0.3.2", "Cost Center 2", url: "/test021"))
        )
);


        context.Menu.SetSubItemOrder(SaasHostMenus.GroupName, 3);

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityProMenus.GroupName, 1);

        //Administration->OpenId
        administration.SetSubItemOrder(OpenIddictProMenus.GroupName, 2);

        //Administration->Language Management
        administration.SetSubItemOrder(LanguageManagementMenus.GroupName, 3);

        //Administration->Text Template Management
        administration.SetSubItemOrder(TextTemplateManagementMenus.GroupName, 4);

        //Administration->Audit Logs
        administration.SetSubItemOrder(AbpAuditLoggingMenus.GroupName, 5);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 6);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                EasiPosStockersMenus.Branches,
                l["Menu:Branches"],
                url: "/branches",
                icon: "fa fa-file-alt",
                requiredPermissionName: EasiPosStockersPermissions.Branches.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                EasiPosStockersMenus.CostCentres,
                l["Menu:CostCentres"],
                url: "/cost-centres",
                icon: "fa fa-file-alt",
                requiredPermissionName: EasiPosStockersPermissions.CostCentres.Default)
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                EasiPosStockersMenus.Products,
                l["Menu:Products"],
                url: "/products",
                icon: "fa fa-file-alt",
                requiredPermissionName: EasiPosStockersPermissions.Products.Default)
        );
        return Task.CompletedTask;
    }

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.SecurityLogs",
            accountStringLocalizer["MySecurityLogs"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/SecurityLogs?returnUrl={_configuration["App:SelfUrl"]}",
            icon: "fa fa-user-shield",
            order: 1001,
            null).RequireAuthenticated());

        await Task.CompletedTask;
    }
}