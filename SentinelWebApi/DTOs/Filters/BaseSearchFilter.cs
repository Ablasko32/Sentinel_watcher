namespace SentinelWebApi.DTOs.Filters
{
    public class BaseSearchFilter
    {
        public int Take
        {
            get => field;
            set => field = value > 100 ? 100 : (value <= 0 ? 10 : value);
        } = 10;
        public int Skip
        {
            get => field;
            set => field = (value > 100) ? 100 : value;
        } = 0;
        public string? SortField { get; set; }

        public string SortOrder { get; set; } = "asc";

        public string? SearchTerm { get; set; }
    }
}
