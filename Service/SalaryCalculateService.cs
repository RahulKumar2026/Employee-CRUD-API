using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class SalaryCalculateService : ISalaryCalculateService
    {
        public decimal CalculateSalary(Employee employee) 
        {
            decimal bonus = employee.Salary * 0.10m;

            return employee.Salary + bonus;
        }
        public decimal CalculateSalary(EmployeeResponseDto employee) 
        {
            decimal bonus = employee.Salary * 0.10m;

            return employee.Salary + bonus;
        }
    }
}
