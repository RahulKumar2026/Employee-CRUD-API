using Employee_CRUD_API.Data;
using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Helper;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Employee_CRUD_API.Repository
{
    public class DepartmentRepository :IDepartmentRepository
    {
        private readonly AppDbContext _dbcontext;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly Sorting _sorting;

        public DepartmentRepository(ILogger<EmployeeRepository> logger, AppDbContext dbcontext, Sorting sorting)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _sorting = sorting;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Calling database from repository layer");
                var result = await _dbcontext.Departments.ToListAsync();
                return result;
            }
            catch (Exception) 
            {
                _logger.LogError("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<Department> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Calling database from repository layer");
                var result = _dbcontext.Departments.FirstOrDefault(d => d.DepartmentId == id);
                return result;
            }
            catch (Exception)
            {
                _logger.LogError("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<List<Department>> AddAsync(List<Department> department) 
        {
            try
            {
                _logger.LogInformation("Calling database from repository layer");
                await _dbcontext.Departments.AddRangeAsync(department);
                await _dbcontext.SaveChangesAsync();
                return department;
            }
            catch (Exception)
            {
                _logger.LogError("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<Department> UpdateAsync(Department department) 
        {
            try
            {
                if (department == null)
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    throw new ArgumentNullException(nameof(department));

                }
                 _dbcontext.Departments.Update(department);
                await _dbcontext.SaveChangesAsync();

                return department;
            }
            catch (Exception) 
            {
                _logger.LogError("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
        public async Task<Department?> DeleteAsync(int id) 
        {
            try
            {
               
                var department = await _dbcontext.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
                _dbcontext.Departments.Remove(department);

                await _dbcontext.SaveChangesAsync();

                return department;
            }
            catch (Exception) 
            {
                _logger.LogError("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                throw;
            }
        }
    }
}
