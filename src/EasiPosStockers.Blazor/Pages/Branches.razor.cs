using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using EasiPosStockers.Branches;
using EasiPosStockers.Permissions;
using EasiPosStockers.Shared;


namespace EasiPosStockers.Blazor.Pages
{
    public partial class Branches
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<BranchDto> BranchList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateBranch { get; set; }
        private bool CanEditBranch { get; set; }
        private bool CanDeleteBranch { get; set; }
        private BranchCreateDto NewBranch { get; set; }
        private Validations NewBranchValidations { get; set; } = new();
        private BranchUpdateDto EditingBranch { get; set; }
        private Validations EditingBranchValidations { get; set; } = new();
        private Guid EditingBranchId { get; set; }
        private Modal CreateBranchModal { get; set; } = new();
        private Modal EditBranchModal { get; set; } = new();
        private GetBranchesInput Filter { get; set; }
        private DataGridEntityActionsColumn<BranchDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "branch-create-tab";
        protected string SelectedEditTab = "branch-edit-tab";
        private BranchDto? SelectedBranch;
        
        
        
        
        public Branches()
        {
            NewBranch = new BranchCreateDto();
            EditingBranch = new BranchUpdateDto();
            Filter = new GetBranchesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            BranchList = new List<BranchDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
            
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Branches"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewBranch"], async () =>
            {
                await OpenCreateBranchModalAsync();
            }, IconName.Add, requiredPolicyName: EasiPosStockersPermissions.Branches.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateBranch = await AuthorizationService
                .IsGrantedAsync(EasiPosStockersPermissions.Branches.Create);
            CanEditBranch = await AuthorizationService
                            .IsGrantedAsync(EasiPosStockersPermissions.Branches.Edit);
            CanDeleteBranch = await AuthorizationService
                            .IsGrantedAsync(EasiPosStockersPermissions.Branches.Delete);
                            
                            
        }

        private async Task GetBranchesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await BranchesAppService.GetListAsync(Filter);
            BranchList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetBranchesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await BranchesAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("EasiPosStockers") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/branches/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}&BranchReference={Filter.BranchReference}&BranchName={Filter.BranchName}&TaxRegistrationNumber={Filter.TaxRegistrationNumber}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<BranchDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetBranchesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateBranchModalAsync()
        {
            NewBranch = new BranchCreateDto{
                
                
            };
            await NewBranchValidations.ClearAll();
            await CreateBranchModal.Show();
        }

        private async Task CloseCreateBranchModalAsync()
        {
            NewBranch = new BranchCreateDto{
                
                
            };
            await CreateBranchModal.Hide();
        }

        private async Task OpenEditBranchModalAsync(BranchDto input)
        {
            var branch = await BranchesAppService.GetAsync(input.Id);
            
            EditingBranchId = branch.Id;
            EditingBranch = ObjectMapper.Map<BranchDto, BranchUpdateDto>(branch);
            await EditingBranchValidations.ClearAll();
            await EditBranchModal.Show();
        }

        private async Task DeleteBranchAsync(BranchDto input)
        {
            await BranchesAppService.DeleteAsync(input.Id);
            await GetBranchesAsync();
        }

        private async Task CreateBranchAsync()
        {
            try
            {
                if (await NewBranchValidations.ValidateAll() == false)
                {
                    return;
                }

                await BranchesAppService.CreateAsync(NewBranch);
                await GetBranchesAsync();
                await CloseCreateBranchModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditBranchModalAsync()
        {
            await EditBranchModal.Hide();
        }

        private async Task UpdateBranchAsync()
        {
            try
            {
                if (await EditingBranchValidations.ValidateAll() == false)
                {
                    return;
                }

                await BranchesAppService.UpdateAsync(EditingBranchId, EditingBranch);
                await GetBranchesAsync();
                await EditBranchModal.Hide();                
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

        protected virtual async Task OnBranchReferenceChangedAsync(string? branchReference)
        {
            Filter.BranchReference = branchReference;
            await SearchAsync();
        }
        protected virtual async Task OnBranchNameChangedAsync(string? branchName)
        {
            Filter.BranchName = branchName;
            await SearchAsync();
        }
        protected virtual async Task OnTaxRegistrationNumberChangedAsync(string? taxRegistrationNumber)
        {
            Filter.TaxRegistrationNumber = taxRegistrationNumber;
            await SearchAsync();
        }


        private async Task OpenBranchCostCentres(BranchDto input)
        {
            // Assuming BranchesAppService.GetAsync returns a Task
            await BranchesAppService.GetAsync(input.Id);

            // Assuming NavigationManager is injected into your component
            Guid id = input.Id;

            NavigationManager.NavigateTo($"/branchCostCentres/{id}");


            Console.WriteLine("----------------------Opening branch centers with id:" + input.Id);
        }



    }
}
