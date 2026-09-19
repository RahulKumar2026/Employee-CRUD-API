using Employee_CRUD_API.Service;
using Employee_CRUD_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_CRUD_API.Controllers
{
    [ApiController]
    [Route("api/repot")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportService _reportService;
        private readonly IGenerateExcelReport _generateExcelReport;
        private readonly IGeneratePdfReport _generatePdfReport;
        public ReportController(ILogger<ReportController> logger, IReportService reportService, IGenerateExcelReport generateExcelReport, IGeneratePdfReport generatePdfReport)
        {
            _logger = logger;
            _reportService = reportService;
            _generateExcelReport = generateExcelReport;
            _generatePdfReport = generatePdfReport;
        }
        [HttpGet("getReport")]
        public async Task<IActionResult> GetEmployeeReportAsync() 
        {
            try
            {
                _logger.LogInformation("calling service layer");
                var result = await _reportService.GetEmployeeReportAsync();
                return Ok(result);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpGet("employee-report/excel")]
        public async Task<IActionResult> DownloadExcel()
        {
            try
            {
                _logger.LogInformation("calling service layer");
                var file = await _generateExcelReport.GenerateExcelReportAsync();
                return File(file,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","EmployeeReport.xlsx");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpGet("employee-report/pdf")]
        public async Task<IActionResult> DownloadPdf()
        {
            try 
            {
                var file = await _generatePdfReport.GeneratePdfReportAsync();

                if (file == null)
                {
                    return NotFound();
                }

                return File(file, "application/pdf", "EmployeeReport.pdf");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
    }
}
