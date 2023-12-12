namespace EasiPosStockers.CostCentres
{
    public static class CostCentreConsts
    {
        private const string DefaultSorting = "{0}CostCentreReference asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "CostCentre." : string.Empty);
        }

        public const int CostCentreReferenceMaxLength = 20;
        public const int CostCentreNameMaxLength = 200;
    }
}