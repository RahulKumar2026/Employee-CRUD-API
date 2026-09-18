using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository;
using Employee_CRUD_API.Repository.DTOs;
using Employee_CRUD_API.Repository.Interface;
using Employee_CRUD_API.Service.Interface;
using System.IO.Pipelines;
using System.Linq.Expressions;
namespace Employee_CRUD_API.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger) 
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        public async Task<List<EmployeeResponseDto>> GetAllAsync(PaginationDto pagination)
        {
            try
            {
                _logger.LogInformation("Calling repostiory Layer");
                //calling repository layer
                var result = await _employeeRepository.GetAllAsync(pagination);

                //vaildation
                if (result == null || !result.Any())
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new List<EmployeeResponseDto>();
                }


                //DTO Mapping
                _logger.LogInformation("DTO Mapping");
                var response =  result.Select(e => new EmployeeResponseDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeName = e.EmployeeName,
                    Salary = e.Salary,
                    Created = e.Created
                }).ToList();


                _logger.LogInformation("Service Layer returing result");
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
                _logger.LogInformation("Calling Repository Layer");
                //calling Repository service
                var result = await _employeeRepository.GetByIdAsync(id);

                //vaildation
                if (result == null)
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);

                    return null;
                }

                //maping to dto
                var response = new EmployeeResponseDto
                {
                    EmployeeId = result.EmployeeId,
                    EmployeeName = result.EmployeeName,
                    Salary = result.Salary,
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
        public async Task<List<EmployeeResponseDto>> AddAsyncList(List<EmployeeRequestDto> request)
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
                     Salary = e.Salary,
                     Created = DateTime.Now,
                }).ToList();

                //calling repository layer
                _logger.LogInformation("Calling Repository Layer");
                bool repoResult = await _employeeRepository.AddAsync(entity);

                if (!repoResult)
                {
                    return new List<EmployeeResponseDto>();
                }
           
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
        public async Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeRequestDto request) 
        {
            try
            {
                // Validation
                if (id <= 0 || request == null)
                {
                    _logger.LogInformation("Invalid employee update request. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);

                    return new EmployeeResponseDto();
                }

                // Mapping DTO to Entity
                var entity = new Employee
                {
                    EmployeeId = id,
                    EmployeeName = request.EmployeeName,
                    DepartmentId = request.DepartmentId,
                    Salary = request.Salary
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
                    Salary = entity.Salary,
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