/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using EasiPosStockers.CostCentres;
using EasiPosStockers.Permissions;
using EasiPosStockers.Shared;


namespace EasiPosStockers.Blazor.Pages
{
    public partial class CostCentreProducts
    {

        public CostCentre()
        {
            NewCostCentre = new CostCentreCreateDto();
            EditingCostCentre = new CostCentreUpdateDto();
            Filter = new GetCostCentresInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            CostCentreList = new List<CostCentreWithNavigationPropertiesDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
            await GetBranchCollectionLookupAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:CostCentres"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () => { await DownloadAsExcelAsync(); }, IconName.Download);

            Toolbar.AddButton(L["NewCostCentre"], async () =>
            {
                await OpenCreateCostCentreModalAsync();
            }, IconName.Add, requiredPolicyName: EasiPosStockersPermissions.CostCentres.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateCostCentre = await AuthorizationService
                .IsGrantedAsync(EasiPosStockersPermissions.CostCentres.Create);
            CanEditCostCentre = await AuthorizationService
                            .IsGrantedAsync(EasiPosStockersPermissions.CostCentres.Edit);
            CanDeleteCostCentre = await AuthorizationService
                            .IsGrantedAsync(EasiPosStockersPermissions.CostCentres.Delete);


        }

        private async Task GetCostCentresAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await CostCentresAppService.GetListAsync(Filter);
            CostCentreList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetCostCentresAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task DownloadAsExcelAsync()
        {
            var token = (await CostCentresAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("EasiPosStockers") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/cost-centres/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}&CostCentreReference={Filter.CostCentreReference}&CostCentreName={Filter.CostCentreName}&IsDisabled={Filter.IsDisabled}&BranchId={Filter.BranchId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CostCentreWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetCostCentresAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateCostCentreModalAsync()
        {
            NewCostCentre = new CostCentreCreateDto
            {


            };
            await NewCostCentreValidations.ClearAll();
            await CreateCostCentreModal.Show();
        }

        private async Task CloseCreateCostCentreModalAsync()
        {
            NewCostCentre = new CostCentreCreateDto
            {


            };
            await CreateCostCentreModal.Hide();
        }

        private async Task OpenEditCostCentreModalAsync(CostCentreWithNavigationPropertiesDto input)
        {
            var costCentre = await CostCentresAppService.GetWithNavigationPropertiesAsync(input.CostCentre.Id);

            EditingCostCentreId = costCentre.CostCentre.Id;
            EditingCostCentre = ObjectMapper.Map<CostCentreDto, CostCentreUpdateDto>(costCentre.CostCentre);
            await EditingCostCentreValidations.ClearAll();
            await EditCostCentreModal.Show();
        }

        private async Task DeleteCostCentreAsync(CostCentreWithNavigationPropertiesDto input)
        {
            await CostCentresAppService.DeleteAsync(input.CostCentre.Id);
            await GetCostCentresAsync();
        }

        private async Task CreateCostCentreAsync()
        {
            try
            {
                if (await NewCostCentreValidations.ValidateAll() == false)
                {
                    return;
                }

                await CostCentresAppService.CreateAsync(NewCostCentre);
                await GetCostCentresAsync();
                await CloseCreateCostCentreModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditCostCentreModalAsync()
        {
            await EditCostCentreModal.Hide();
        }

        private async Task UpdateCostCentreAsync()
        {
            try
            {
                if (await EditingCostCentreValidations.ValidateAll() == false)
                {
                    return;
                }

                await CostCentresAppService.UpdateAsync(EditingCostCentreId, EditingCostCentre);
                await GetCostCentresAsync();
                await EditCostCentreModal.Hide();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }

        protected virtual async Task OnCostCentreReferenceChangedAsync(string? costCentreReference)
        {
            Filter.CostCentreReference = costCentreReference;
            await SearchAsync();
        }
        protected virtual async Task OnCostCentreNameChangedAsync(string? costCentreName)
        {
            Filter.CostCentreName = costCentreName;
            await SearchAsync();
        }
        protected virtual async Task OnIsDisabledChangedAsync(bool? isDisabled)
        {
            Filter.IsDisabled = isDisabled;
            await SearchAsync();
        }
        protected virtual async Task OnBranchIdChangedAsync(Guid? branchId)
        {
            Filter.BranchId = branchId;
            await SearchAsync();
        }


        private async Task GetBranchCollectionLookupAsync(string? newValue = null)
        {
            BranchesCollection = (await CostCentresAppService.GetBranchLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }


        private async Task OpenCostCentreProductsAsync(CostCentreWithNavigationPropertiesDto input)
        {
            // Assuming BranchesAppService.GetAsync returns a Task
            await CostCentresAppService.GetAsync(input.CostCentre.Id);

            // Assuming NavigationManager is injected into your component
            Guid id = input.CostCentre.Id;
            String costCentreReference = input.CostCentre.CostCentreReference;
            String costCentreName = input.CostCentre.CostCentreName;
            bool isDisabled = input.CostCentre.IsDisabled;
            String branchName = input.Branch.BranchName;
            String concurrencyStamp = input.CostCentre.ConcurrencyStamp;

            NavigationManager.NavigateTo($"/costCentreProducts/{id}/{costCentreReference}/{costCentreName}/{isDisabled}/{branchName}/{concurrencyStamp}");


            Console.WriteLine("\n\nOpening cost centre with id:" + input.CostCentre.Id + "\n\n");
        }


    }
}
*/