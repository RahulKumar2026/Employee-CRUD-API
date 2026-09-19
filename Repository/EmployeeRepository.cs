using Employee_CRUD_API.Data;
using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Helper;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.DTOs;
using Employee_CRUD_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

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
        public async Task<List<EmployeeResponseDto>> GetAllAsync() 
        {
            try
            {
                _logger.LogInformation("Repository is accessing data from database");
                var query = await (from em in _dbcontext.Employees.AsNoTracking()
                                    join dp in _dbcontext.Departments.AsNoTracking() on em.DepartmentId equals dp.DepartmentId
                                    into departmentGroup
                                    from dn in departmentGroup.DefaultIfEmpty()
                                    select new EmployeeResponseDto
                                    {
                                        EmployeeId = em.EmployeeId,
                                        EmployeeName = em.EmployeeName,
                                        Salary = em.Salary,
                                        Created = em.Created,
                                        DepartmentId = em.DepartmentId,
                                        DepartmentName = dn != null ? dn.DepartmentName : null
                                    }).ToListAsync();

                _logger.LogInformation("Pagination done!");
                return query;
            }
            catch (Exception)
            {
                _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<EmployeeResponseDto?> GetByIdAsync(int id) 
        {
            try
            {
                _logger.LogInformation("Repository is accessing data from database");
                var employee = await (from em in _dbcontext.Employees.AsNoTracking().Where(e => e.EmployeeId == id)
                                      join dp in _dbcontext.Departments.AsNoTracking()
                                      on em.DepartmentId equals dp.DepartmentId
                                      into departmentGroup
                                      from dn in departmentGroup.DefaultIfEmpty()
                                      select new EmployeeResponseDto
                                      {
                                          EmployeeId = em.EmployeeId,
                                          EmployeeName = em.EmployeeName,
                                          Salary = em.Salary,
                                          Created = em.Created,
                                          DepartmentId = em.DepartmentId,
                                          DepartmentName = dn != null ? dn.DepartmentName : null
                                      }).FirstOrDefaultAsync();



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
                _logger.LogInformation("Repository is accessing data from database");
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
                _logger.LogInformation("Repository is accessing data from database");
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
