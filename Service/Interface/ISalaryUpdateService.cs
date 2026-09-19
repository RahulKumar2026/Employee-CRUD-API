using Employee_CRUD_API.Repository.DTOs;

namespace Employee_CRUD_API.Service.Interface
{
    public interface ISalaryUpdateService
    {
        decimal CalculateSalary(EmployeeUpdateDto request);
        decimal CalculateSalary(EmployeeCreateDto request);
    }
}
