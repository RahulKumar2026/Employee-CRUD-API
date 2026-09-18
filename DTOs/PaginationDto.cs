namespace Employee_CRUD_API.DTOs
{
    public class PaginationDto
    {
        const int maxPageSize = 10;

        public string? searchItem { get; set; } = "";

        public int PageNumber { get; set; } = 1;

        public string? SortColumn { get; set; }

        public string? SortDirection { get; set; } = "desc";

        public int _pageSize { get; set; } = 10;

        public int pageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}