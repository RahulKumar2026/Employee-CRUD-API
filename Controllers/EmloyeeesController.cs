using Employee_CRUD_API.Data;
using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Repository.DTOs;
using Employee_CRUD_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_CRUD_API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmloyeeesController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        private readonly ILogger<EmloyeeesController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmloyeeesController(ILogger<EmloyeeesController> logger, AppDbContext dbcontext, IEmployeeService employeeService)
        {
            _logger = logger;
            _dbcontext = dbcontext;
            _employeeService = employeeService;
        }
        [HttpGet("GetAllEmp")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto? pagination)
        {
            try
            {
                _logger.LogInformation("calling service layer");
                var response = await _employeeService.GetAllAsync(pagination);
                //returning response
                return Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpGet("GetEmpById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery]int id) 
        {
            try
            {
                _logger.LogInformation("Calling Service Layer");
                var reponse = await _employeeService.GetByIdAsync(id);
                //returning response
                return Ok(reponse);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpPost("addEmp")]
        public async Task<IActionResult> AddAsyncList([FromBody]List<EmployeeCreateDto> request)
        {
            try 
            {
                _logger.LogInformation("Calling Service Layer");
                var response = await _employeeService.AddAsyncList(request);
                // returning response
                return Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }

        [HttpPut("UpdateEmp")]
        public async Task<IActionResult> UpdateAsync([FromQuery] int id, [FromBody] EmployeeUpdateDto request) 
        {
            try
            {
                _logger.LogInformation("Calling Service Layer");
                var response = await _employeeService.UpdateAsync(id,request);
                // returning response
                return Ok(response);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        
        [HttpDelete("DeleteEmpById")]
        public async Task<IActionResult> DelteAsync([FromQuery] int id) 
        {
            try
            {
                _logger.LogInformation("Calling Service Layer");
                var response = await _employeeService.DeleteAsync(id);
                // returning response
                return Ok(new { Message = "data deleted succssfully", status=response });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
    }
}
