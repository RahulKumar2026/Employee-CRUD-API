using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.DTOs;
namespace Employee_CRUD_API.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<List<EmployeeResponseDto>> GetAllAsync();
        Task<EmployeeResponseDto?> GetByIdAsync(int id);
        Task<bool> AddAsync(List<Employee> request);
        Task<bool> UpdateAsync(Employee request);
        Task<bool> DeleteAsync(int id);
    }
}
