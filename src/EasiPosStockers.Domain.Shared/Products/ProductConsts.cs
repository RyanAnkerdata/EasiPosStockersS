namespace EasiPosStockers.Products
{
    public static class ProductConsts
    {
        private const string DefaultSorting = "{0}Description asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Product." : string.Empty);
        }

        public const int DescriptionMaxLength = 200;
        public const int ProductNameMaxLength = 100;
    }
}