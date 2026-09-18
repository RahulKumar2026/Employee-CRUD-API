using Employee_CRUD_API.Models;

namespace Employee_CRUD_API.Helper
{
    public class Sorting
    {
        public IQueryable<Employee> SortListing(IQueryable<Employee> data, string sortColumn, string sortDirection)
        {
            string sortBy = sortColumn?.Trim().ToLower() ?? "employeeid";
            bool isAsc = sortDirection?.Trim().ToLower() == "asc";

            switch (sortBy)
            {
                case "employeeid":
                    return isAsc
                        ? data.OrderBy(x => x.EmployeeId)
                        : data.OrderByDescending(x => x.EmployeeId);
                case "employeename":
                    return isAsc
                        ? data.OrderBy(x => x.EmployeeName)
                        : data.OrderByDescending(x => x.EmployeeName);
                case "salary":
                    return isAsc
                        ? data.OrderBy(x => x.Salary)
                        : data.OrderByDescending(x => x.Salary);
                case "created":
                    return isAsc
                        ? data.OrderBy(x => x.Created)
                        : data.OrderByDescending(x => x.Created);
                default:
                    return isAsc
                        ? data.OrderBy(x => x.EmployeeId)
                        : data.OrderByDescending(x => x.EmployeeId);
            }
        }
    }
}
