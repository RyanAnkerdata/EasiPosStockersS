namespace EasiPosStockers.Branches
{
    public static class BranchConsts
    {
        private const string DefaultSorting = "{0}BranchReference asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Branch." : string.Empty);
        }

        public const int BranchReferenceMaxLength = 20;
        public const int BranchNameMaxLength = 20;
        public const int TaxRegistrationNumberMaxLength = 20;
    }
}