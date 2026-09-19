using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;

namespace Employee_CRUD_API.Service.Interface
{
    public interface IDepartmentService
    {
        Task<List<DepartmentResponseDto>> GetAllAsync();

        Task<DepartmentResponseDto> GetByIdAsync(int id);

        Task<List<DepartmentResponseDto>> AddAsync(List<DepartmentCreateDto> request);

        Task<DepartmentResponseDto?> UpdateAsync(int id,DepartmentUpdateDto request);

        Task<DepartmentResponseDto> DeleteAsync(int id);
    }
}
