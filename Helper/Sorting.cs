using Employee_CRUD_API.DTOs;

namespace Employee_CRUD_API.Helper
{
    public class Sorting
    {
        public List<EmployeeResponseDto> SortListing(List<EmployeeResponseDto> data, string sortColumn, string sortDirection)
        {
            string sortBy = sortColumn?.Trim().ToLower() ?? "employeeid";
            bool isAsc = sortDirection?.Trim().ToLower() == "asc";

            switch (sortBy)
            {
                case "employeeid":
                    return isAsc
                        ? data.OrderBy(x => x.EmployeeId).ToList()
                        : data.OrderByDescending(x => x.EmployeeId).ToList();
                case "employeename":
                    return isAsc
                        ? data.OrderBy(x => x.EmployeeName).ToList()
                        : data.OrderByDescending(x => x.EmployeeName).ToList();
                case "salary":
                    return isAsc
                        ? data.OrderBy(x => x.Salary).ToList()
                        : data.OrderByDescending(x => x.Salary).ToList();
                case "created":
                    return isAsc
                        ? data.OrderBy(x => x.Created).ToList()
                        : data.OrderByDescending(x => x.Created).ToList();
                default:
                    return isAsc
                        ? data.OrderBy(x => x.EmployeeId).ToList()
                        : data.OrderByDescending(x => x.EmployeeId).ToList();
            }
        }
    }
}
