namespace ShopperAPi.Resources_Params
{
    public class ProductParams
    {
        public string? CategoryName { get; set; }
        public int? rate { get; set; }
        public int? PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > 10) ? 10 : value;
        }
        public int? maxPrice { get; set; }
        public int? miniPrice { get; set; }

    }
}
