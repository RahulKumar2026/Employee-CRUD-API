using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;

namespace Employee_CRUD_API.Repository.Interface
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task<List<Department>> AddAsync(List<Department> department);
        Task<Department> UpdateAsync(Department department);
        Task<Department?> DeleteAsync(int id);
    }
}
