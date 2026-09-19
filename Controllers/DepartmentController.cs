using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_CRUD_API.Controllers
{
    [ApiController]
    [Route("api/department")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger) 
        {
            _departmentService = departmentService;
            _logger = logger;
        }
        [HttpGet("GetAllDep")]
        public async Task<IActionResult> GetAllAsync() 
        {
            try
            {
                _logger.LogInformation("Calling to the service layer");
                var response = await _departmentService.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpGet("GetDepById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id) 
        {
            try
            {
                _logger.LogInformation("Calling to the service layer");
                var response = await _departmentService.GetByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] List<DepartmentCreateDto> request) 
        {
            try
            {
                _logger.LogInformation("Calling to the service layer");
                var response = await _departmentService.AddAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id, [FromBody] DepartmentUpdateDto request)
        {
            try
            {
                _logger.LogInformation("Calling to the service layer");
                var response = await _departmentService.UpdateAsync(id, request);
                return Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] int id) 
        {
            try
            {
                _logger.LogInformation("Calling to the service layer");
                var response = await _departmentService.DeleteAsync(id);
                return Ok(new {Message = "Data Deleted Successfully" ,response});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
    }
}
