using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Repository.DTOs;

namespace Employee_CRUD_API.Service.Interface
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllAsync(PaginationDto? pagination);
        Task<EmployeeResponseDto?> GetByIdAsync(int id);
        Task<List<EmployeeResponseDto>> AddAsyncList(List<EmployeeCreateDto> request);
        Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeUpdateDto request);
        Task<bool> DeleteAsync(int id);
    }
}
