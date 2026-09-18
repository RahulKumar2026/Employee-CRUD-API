using Employee_CRUD_API.Data;
using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Helper;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.DTOs;
using Employee_CRUD_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Employee_CRUD_API.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _dbcontext;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly Sorting _sorting;

        public EmployeeRepository(ILogger<EmployeeRepository> logger, AppDbContext dbcontext, Sorting sorting)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _sorting = sorting;
        }
        public async Task<List<Employee>> GetAllAsync(PaginationDto pagination) 
        {
            try
            {
                _logger.LogInformation("Repository is accessing data from database");
                var query = _dbcontext.Employees.AsNoTracking();
                

                if (!string.IsNullOrWhiteSpace(pagination.searchItem))
                {
                    var searchItem = pagination.searchItem.Trim();

                    query = query.Where(x =>
                        EF.Functions.ILike(x.EmployeeName!, $"%{searchItem}%") ||
                        EF.Functions.ILike(x.EmployeeId.ToString(), $"%{searchItem}%") ||
                        EF.Functions.ILike(x.Salary.ToString(), $"%{searchItem}%") ||
                        EF.Functions.ILike(x.Created.ToString(), $"%{searchItem}%")
                    );
                }

                // Sorting
                query = _sorting.SortListing(query, pagination.SortColumn, pagination.SortDirection);


                //Pagination
                var result = await query
                    .Skip((pagination.PageNumber - 1) * pagination.pageSize)
                    .Take(pagination.pageSize)
                    .ToListAsync();

                _logger.LogInformation("Pagination done!");
                return result;
            }
            catch (Exception)
            {
                _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<Employee?> GetByIdAsync(int id) 
        {
            try
            {
                _logger.LogInformation("Repository is accessing data from database");
                var employee = await _dbcontext.Employees.AsNoTracking()
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

                return employee;

            }
            catch (Exception)
            {
                _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<bool> AddAsync(List<Employee> request) 
        {
            try
            {
                _logger.LogInformation("Repository is accessing data from database");
                await _dbcontext.Employees.AddRangeAsync(request);
                 await _dbcontext.SaveChangesAsync();
                 return true;
            }
            catch (Exception)
            {
                _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<bool> UpdateAsync(Employee request) 
        {
            try
            {
                if (request == null)
                {
                    _logger.LogInformation( "Employee data not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);

                    return false;
                }

                _dbcontext.Employees.UpdateRange(request);

                await _dbcontext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id) 
        {
            try
            {
                var employee = await _dbcontext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
                if (employee == null)
                {
                    _logger.LogInformation("Employee not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);

                    return false;
                }

                _dbcontext.Employees.Remove(employee);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception) 
            {
                _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
    }
}
