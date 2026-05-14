
namespace E_commerce.Shared.Common.Params
{
    public class BaseQueryParams
    {
        private const int MaxPageSize = 20;

        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value < 1) ? 6 : (value > MaxPageSize) ? MaxPageSize : value;
        }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = (value < 1) ? 1 : value;
        }

        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }
    }
}
