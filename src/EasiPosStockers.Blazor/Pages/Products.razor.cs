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
using EasiPosStockers.Products;
using EasiPosStockers.Permissions;
using EasiPosStockers.Shared;
using EasiPosStockers.CostCentres;
using Excubo.Generators.Blazor.ExperimentalDoNotUseYet;


namespace EasiPosStockers.Blazor.Pages
{
    public partial class Products
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        protected bool ShowAdvancedFilters { get; set; }
        private IReadOnlyList<ProductWithNavigationPropertiesDto> ProductList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }
        private bool CanCreateProduct { get; set; }
        private bool CanEditProduct { get; set; }
        private bool CanDeleteProduct { get; set; }
        private ProductCreateDto NewProduct { get; set; }
        private Validations NewProductValidations { get; set; } = new();
        private ProductUpdateDto EditingProduct { get; set; }
        private Validations EditingProductValidations { get; set; } = new();
        private Guid EditingProductId { get; set; }
        private Modal CreateProductModal { get; set; } = new();
        private Modal EditProductModal { get; set; } = new();
        private GetProductsInput Filter { get; set; }
        private DataGridEntityActionsColumn<ProductWithNavigationPropertiesDto> EntityActionsColumn { get; set; } = new();
        protected string SelectedCreateTab = "product-create-tab";
        protected string SelectedEditTab = "product-edit-tab";
        private ProductWithNavigationPropertiesDto? SelectedProduct;
        private IReadOnlyList<LookupDto<Guid>> CostCentres { get; set; } = new List<LookupDto<Guid>>();
        
        private string SelectedCostCentreId { get; set; }
        
        private string SelectedCostCentreText { get; set; }

        private List<LookupDto<Guid>> SelectedCostCentres { get; set; } = new List<LookupDto<Guid>>();
        
        
        
        public Products()
        {
            NewProduct = new ProductCreateDto();
            EditingProduct = new ProductUpdateDto();
            Filter = new GetProductsInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
            ProductList = new List<ProductWithNavigationPropertiesDto>();
            
            
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
            await GetAllCostCentresAsync();
            
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Products"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewProduct"], async () =>
            {
                await OpenCreateProductModalAsync();
            }, IconName.Add, requiredPolicyName: EasiPosStockersPermissions.Products.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateProduct = await AuthorizationService
                .IsGrantedAsync(EasiPosStockersPermissions.Products.Create);
            CanEditProduct = await AuthorizationService
                            .IsGrantedAsync(EasiPosStockersPermissions.Products.Edit);
            CanDeleteProduct = await AuthorizationService
                            .IsGrantedAsync(EasiPosStockersPermissions.Products.Delete);    
        }

        private async Task GetProductsAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;
            Console.WriteLine("About to execute GetListAsync Function!\n\nKind regards!");
            var result = await ProductsAppService.GetListAsync(Filter);
            Console.WriteLine("Finished executing GetListAsync Function!\n\nKind regards!");

            ProductList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetProductsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await ProductsAppService.GetDownloadTokenAsync()).Token;
            var remoteService = await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("EasiPosStockers") ??
            await RemoteServiceConfigurationProvider.GetConfigurationOrDefaultOrNullAsync("Default");
            NavigationManager.NavigateTo($"{remoteService?.BaseUrl.EnsureEndsWith('/') ?? string.Empty}api/app/products/as-excel-file?DownloadToken={token}&FilterText={Filter.FilterText}&Description={Filter.Description}&ProductName={Filter.ProductName}&CostCentreId={Filter.CostCentreId}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ProductWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetProductsAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateProductModalAsync()
        {
            SelectedCostCentres = new List<LookupDto<Guid>>();


            NewProduct = new ProductCreateDto
            {


            };
            await NewProductValidations.ClearAll();
            await CreateProductModal.Show();
        }

        private async Task CloseCreateProductModalAsync()
        {
            NewProduct = new ProductCreateDto{
                
                
            };
            await CreateProductModal.Hide();
        }

        private async Task OpenEditProductModalAsync(ProductWithNavigationPropertiesDto input)
        {
            var product = await ProductsAppService.GetWithNavigationPropertiesAsync(input.Product.Id);
            
            EditingProductId = product.Product.Id;
            EditingProduct = ObjectMapper.Map<ProductDto, ProductUpdateDto>(product.Product);

            // KEY TO THE EDIT MODAL REMEMBERING THE SELECTED COST CENTRES FROM THE CREATE MODAL

            SelectedCostCentres = product.CostCentres.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.CostCentreName}).ToList();

            await EditingProductValidations.ClearAll();
            await EditProductModal.Show();
        }

        private async Task DeleteProductAsync(ProductWithNavigationPropertiesDto input)
        {
            input.CostCentres = new List<CostCentreDto>();
            await ProductsAppService.DeleteAsync(input.Product.Id);
            await GetProductsAsync();
        }

        private async Task CreateProductAsync()
        {
            try
            {
                if (await NewProductValidations.ValidateAll() == false)
                {
                    return;
                }
                NewProduct.CostCentreIds = SelectedCostCentres.Select(x => x.Id).ToList();
                await ProductsAppService.CreateAsync(NewProduct);
                await GetProductsAsync();
                await CloseCreateProductModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditProductModalAsync()
        {
            await EditProductModal.Hide();
        }



        private async Task UpdateProductAsync()
        {
            try
            {
                if (await EditingProductValidations.ValidateAll() == false)
                {
                    return;
                }

                EditingProduct.CostCentreIds = SelectedCostCentres.Select(x => x.Id).ToList();
                Console.WriteLine($"\n\nNumber 1...\n\n");

                await ProductsAppService.UpdateAsync(EditingProductId, EditingProduct);
                Console.WriteLine($"\n\nNumber 2...\n\n");

                await GetProductsAsync();
                await EditProductModal.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nThe following EXCEPTION was thrown bad boy:\n\n{ex}\n\n");
                await HandleErrorAsync(ex);
            }
        }


        /*private async Task UpdateProductAsync()
        {
            try
            {
                if (await EditingProductValidations.ValidateAll() == false)
                {
                    return;
                }
                Console.WriteLine("\n\n\n1\n\n\n");
                Console.WriteLine($"SelectedCostCentres:");
                foreach (var item in SelectedCostCentres)
                {
                    Console.WriteLine($"CostCentreId = {item.Id}");
                }
                Console.WriteLine("\n\n\nEND SELECTED COST CENTRES\n\n\n");


                EditingProduct.CostCentreIds = SelectedCostCentres.Select(x => x.Id).ToList();

                // if cost centre id in productcostcentres table is not in the costcentreids list, remove it from the productcostcentres table

                Console.WriteLine("\n\n\n1\n\n\n");
                Console.WriteLine($"CostCentreIds:");
                foreach (var item in EditingProduct.CostCentreIds)
                {
                    Console.WriteLine($"CostCentreId = {item}");
                }
                Console.WriteLine("\n\n\nEND\n\n\n");

                //remove the cost centres from the ProductCostCentres table
                //await ProductsAppService.RemoveCostCentresFromProductAsync(EditingProductId, EditingProduct.CostCentreIds);

                await ProductsAppService.UpdateAsync(EditingProductId, EditingProduct); // Problem Statement - This is not updating the CostCentres
                await GetProductsAsync();
                await EditProductModal.Hide();                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\nThe following EXCEPTION was thrown bad boy:\n\n{ex}\n\n");
                await HandleErrorAsync(ex);
            }
        }*/

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }

        protected virtual async Task OnDescriptionChangedAsync(string? description)
        {
            Filter.Description = description;
            await SearchAsync();
        }
        protected virtual async Task OnProductNameChangedAsync(string? productName)
        {
            Filter.ProductName = productName;
            await SearchAsync();
        }
        protected virtual async Task OnCostCentreIdChangedAsync(Guid? costCentreId)
        {
            Filter.CostCentreId = costCentreId;
            await SearchAsync();
        }
        
        private async Task GetAllCostCentresAsync() // Added
        {
            SelectedCostCentres.Clear();
            CostCentres = (await ProductsAppService.GetCostCentreLookupAsync(new LookupRequestDto { Filter = "" })).Items;
        }

        private async Task GetCostCentreLookupAsync(string? newValue = null)
        {
            CostCentres = (await ProductsAppService.GetCostCentreLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private void AddCostCentre()
        {
            if (SelectedCostCentreId.IsNullOrEmpty())
            {
                return;
            }
            
            if (SelectedCostCentres.Any(p => p.Id.ToString() == SelectedCostCentreId))
            {
                UiMessageService.Warn(L["ItemAlreadyAdded"]);
                return;
            }

            SelectedCostCentres.Add(new LookupDto<Guid>
            {
                Id = Guid.Parse(SelectedCostCentreId),
                DisplayName = SelectedCostCentreText
            });
        }
    }
}
