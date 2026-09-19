using Employee_CRUD_API.Repository.DTOs;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class SalaryUpdateService : ISalaryUpdateService
    {
        public decimal CalculateSalary(EmployeeUpdateDto request)
        {
            return request.Salary;
        }
        public decimal CalculateSalary(EmployeeCreateDto request) 
        {
            return request.Salary;
        }
    }
}
