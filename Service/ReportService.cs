using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.Interface;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class ReportService : IReportService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportService> _logger;
        public ReportService(IEmployeeRepository employeeRepository, ILogger<ReportService> logger) 
        {
            _employeeRepository = employeeRepository; ;
            _logger = logger;
        }
        public async Task<EmployeeReportDto> GetEmployeeReportAsync() 
        {
            try
            {
                _logger.LogInformation("Generating employee report");
                var employees = await _employeeRepository.GetAllAsync();

                //vaildation
                if (employees == null || !employees.Any())
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new EmployeeReportDto();
                }
                // Business logic
                var response = new EmployeeReportDto
                {
                    TotalEmployees = employees.Count,
                    TotalSalary = employees.Sum(x => x.Salary),
                    AverageSalary = employees.Count > 0 ? employees.Average(x => x.Salary): 0
                };
                _logger.LogInformation("Returing result");
                return response;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while generating employee report");
                throw;
            }
        }
    }
}
