using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Enums;
using Employee_CRUD_API.Helper;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository.DTOs;
using Employee_CRUD_API.Repository.Interface;
using Employee_CRUD_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
namespace Employee_CRUD_API.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly ISalaryCalculateService _salaryCalculateService;
        private readonly ISalaryUpdateService _salaryUpdateService;
        private readonly INotificationService _notificationService;
        private Sorting _sort;
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger, ISalaryCalculateService salaryCalcukateService, ISalaryUpdateService salaryUpdateService, Sorting sort, INotificationService notificationService) 
        {
            _employeeRepository = employeeRepository;
            _salaryCalculateService = salaryCalcukateService;
            _salaryUpdateService = salaryUpdateService;
            _notificationService = notificationService;
            _sort = sort;
            _logger = logger;
        }
        public async Task<List<EmployeeResponseDto>> GetAllAsync(PaginationDto pagination)
        {
            try
            {
                _logger.LogInformation("Calling repostiory Layer");
                //calling repository layer
                var result = await _employeeRepository.GetAllAsync();

                //vaildation
                if (result == null || !result.Any())
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new List<EmployeeResponseDto>();
                }
                // Search
                if (pagination != null && !string.IsNullOrWhiteSpace(pagination.searchItem))
                {
                    var searchItem = pagination.searchItem.Trim();
                    result = result.Where(x =>x.EmployeeName.Contains(searchItem,StringComparison.OrdinalIgnoreCase) ||
                                          x.EmployeeId.ToString().Contains(searchItem) ||
                                          x.Salary.ToString().Contains(searchItem)).ToList();
                }

                // Sorting
                if (pagination != null)
                {
                    result = _sort.SortListing(result,pagination.SortColumn,pagination.SortDirection);
                }

                // Pagination
                if (pagination != null)
                {
                    result = result
                        .Skip((pagination.PageNumber - 1) * pagination.pageSize)
                        .Take(pagination.pageSize)
                        .ToList();
                }

                // Entity → DTO
                var response = result.Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.EmployeeName,
                    DepartmentId = e.DepartmentId,
                    Salary = _salaryCalculateService.CalculateSalary(e),
                    Created = e.Created
                }).ToList();

                _logger.LogInformation("Service layer returning result");

                return response;


            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            try
            {
                //vaildation
                if (id <= 0) 
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);
                    return null;
                }


                _logger.LogInformation("Calling Repository Layer");
                //calling Repository service
                var result = await _employeeRepository.GetByIdAsync(id);

                //vaildation
                if (result == null)
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);

                    return null;
                }

                // Salary business logic
                var calculatedSalary = _salaryCalculateService.CalculateSalary(result);

                //maping to dto
                var response = new EmployeeResponseDto
                {
                    EmployeeId = result.EmployeeId,
                    EmployeeName = result.EmployeeName,
                    Salary = calculatedSalary,
                    DepartmentId = result.DepartmentId,
                    Created = result.Created

                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<List<EmployeeResponseDto>> AddAsyncList(List<EmployeeCreateDto> request)
        {
            try
            {
                // Validation
                if (request == null || !request.Any())
                {
                    _logger.LogInformation( "Employee data not found. Code: {Code}",HTTPResponseWrapper.Constants.BadRequestCode);

                    return new List<EmployeeResponseDto>();
                }

                //mapping dto to entity
                var entity = request.Select(e => new Employee 
                {
                     EmployeeName = e.EmployeeName,
                     DepartmentId = e.DepartmentId,
                     Salary = _salaryUpdateService.CalculateSalary(e),
                     Created = DateTime.Now,
                }).ToList();

                //calling repository layer
                _logger.LogInformation("Calling Repository Layer");
                bool repoResult = await _employeeRepository.AddAsync(entity);

                if (!repoResult)
                {
                    return new List<EmployeeResponseDto>();
                }
                await _notificationService.SendAsync(NotificationType.Email,"rahul@example.com", "Employee Created","Employee records were created successfully.");
                await _notificationService.SendAsync(NotificationType.WhatsApp,"919876543210","Employee Created","Employee Rahul Kumar was created successfully.");
                // Mapping Entity to Response DTO
                var response = entity.Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.EmployeeName,
                    Salary = e.Salary,
                    Created = e.Created
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeUpdateDto request) 
        {
            try
            {
                // Validation
                if (id <= 0 || request == null)
                {
                    _logger.LogInformation("Invalid employee update request. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);

                    return new EmployeeResponseDto();
                }

                // Salary business logic
                var salary = _salaryUpdateService.CalculateSalary(request);

                // Mapping DTO to Entity
                var entity = new Employee
                {
                    EmployeeId = id,
                    EmployeeName = request.EmployeeName,
                    DepartmentId = request.DepartmentId,
                    Salary = salary
                };

                // Calling Repository Layer
                _logger.LogInformation("Calling Repository Layer");

                bool result = await _employeeRepository.UpdateAsync(entity);

                if (!result)
                {
                    _logger.LogInformation("Employee not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new EmployeeResponseDto();
                }

                // Mapping Entity to Response DTO
                var response = new EmployeeResponseDto
                {
                    EmployeeId = entity.EmployeeId,
                    EmployeeName = entity.EmployeeName,
                    Salary = salary,
                    Created = entity.Created
                };

                return response;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id) 
        {
            try
            {
                //vaildate
                if (id <= 0) 
                {
                    _logger.LogInformation("Invalid employee update request. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);
                    return false;
                }
                //calling repository layer
                bool result = await _employeeRepository.DeleteAsync(id);
                if (!result) 
                {
                    _logger.LogInformation("Invalid employee update request. Code: {Code}", HTTPResponseWrapper.Constants.NoRecordExist);
                    return false;
                }
                return true;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
    }
}