using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Models;
using Employee_CRUD_API.Repository;
using Employee_CRUD_API.Repository.Interface;
using Employee_CRUD_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
namespace Employee_CRUD_API.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILogger<DepartmentService> _logger;
        public DepartmentService(IDepartmentRepository departmentRepository, ILogger<DepartmentService> logger) 
        {
            _departmentRepository = departmentRepository;
            _logger = logger;
        }
        public async Task<List<DepartmentResponseDto>> GetAllAsync() 
        {
            try
            {
                _logger.LogInformation("Calling repository layer");
                var result = await _departmentRepository.GetAllAsync();

                //vaildation
                if (!result.Any()) 
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new List<DepartmentResponseDto>();
                }

                //Mapping Entity to dto
                var response =  result.Select(d => new DepartmentResponseDto 
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                }).ToList();

                //Retrun response
                _logger.LogInformation("Returing response to the controller");
                return response;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<DepartmentResponseDto> GetByIdAsync(int id)
        {
            try
            {
                //vaildation
                if (id >= 0)
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);
                    return new DepartmentResponseDto();
                }
                _logger.LogInformation("Calling repository layer");
                var result = await _departmentRepository.GetByIdAsync(id);

                if (result == null)
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new DepartmentResponseDto();
                }

                var response = new DepartmentResponseDto
                {
                    DepartmentId = result.DepartmentId,
                    DepartmentName = result.DepartmentName
                };

                //Retrun response
                _logger.LogInformation("Returing response to the controller");
                return response;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<List<DepartmentResponseDto>> AddAsync(List<DepartmentCreateDto> request) 
        {
            try
            {
                //vaildation
                if (!request.Any()) 
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);
                    return new List<DepartmentResponseDto>();
                }
                //DTO to entity
                var Entity = request.Select(d => new Department{
                     DepartmentName = d.DepartmentName,
                }).ToList();

                //calling repository layer
                _logger.LogInformation("Calling to the repository layer");
                var result = await _departmentRepository.AddAsync(Entity);

                //vaildation 
                if (!result.Any())
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return new List<DepartmentResponseDto>();
                }

                //Maping Entity to DTO

                var response = result.Select(d => new DepartmentResponseDto { 
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                }).ToList();

                //Retrun response
                _logger.LogInformation("Returing response to the controller");
                return response;

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<DepartmentResponseDto?> UpdateAsync(int id, DepartmentUpdateDto request) 
        {
            try
            {
                    // Validation
                    if (id <= 0 || request == null)
                    {
                        _logger.LogInformation("Invalid employee update request. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);

                        return new DepartmentResponseDto();
                    }

                    // Mapping DTO to Entity
                    var entity = new Department
                    {
                        DepartmentName = request.DepartmentName,
                        DepartmentId = id
                    };

                    //calling repository layer
                    _logger.LogInformation("Calling to the repository layer");
                    var result = await _departmentRepository.UpdateAsync(entity);

                    //vaildation 
                    if (result == null)
                    {
                        _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                        throw new ArgumentNullException(nameof(result));
                    }

                //Dto to Entity
                var response = new DepartmentResponseDto
                {
                    DepartmentId = result.DepartmentId,
                    DepartmentName = result.DepartmentName
                };

                return response;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
        public async Task<DepartmentResponseDto> DeleteAsync(int id)
        {
            try
            {
                // Validation
                if (id <= 0)
                {
                    _logger.LogInformation("Invalid employee update request. Code: {Code}", HTTPResponseWrapper.Constants.BadRequestCode);
                    return null;
                }

                //calling repository layer
                var result = await _departmentRepository.DeleteAsync(id);

                if (result == null) 
                {
                    _logger.LogInformation("Employee data not found. Code: {Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    throw new ArgumentNullException(nameof(result));
                }

                //Dto to Entity
                var response = new DepartmentResponseDto
                {
                    DepartmentId = result.DepartmentId,
                    DepartmentName = result.DepartmentName
                };

                return response;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while accessing repository layer");
                throw;
            }
        }
    }
}
