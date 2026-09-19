using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;

namespace Employee_CRUD_API.Service.Interface
{
    public interface ISalaryCalculateService
    {
        decimal CalculateSalary(Employee employee);
        decimal CalculateSalary(EmployeeResponseDto employee);

    }
}
