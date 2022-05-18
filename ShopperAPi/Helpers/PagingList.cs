namespace ShopperAPi.Helpers
{
    public class PagingList<T>
    {
        public PagingList(int pageIndex, int pageSize, int count, IReadOnlyList<T> result)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Result = result;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Result { get; set; }
    }
}
