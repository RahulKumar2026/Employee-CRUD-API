using Employee_CRUD_API.Enums;
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
        private readonly IEmployeeReportEmailService _employeeReportEmailService;
        private readonly INotificationService _notificationService;
        public ReportController(ILogger<ReportController> logger, IReportService reportService, IGenerateExcelReport generateExcelReport, IGeneratePdfReport generatePdfReport, IEmployeeReportEmailService employeeReportEmailService, INotificationService notificationService)
        {
            _logger = logger;
            _reportService = reportService;
            _generateExcelReport = generateExcelReport;
            _generatePdfReport = generatePdfReport;
            _employeeReportEmailService = employeeReportEmailService;
            _notificationService = notificationService;
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
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeReport.xlsx");
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
                return File(file, "application/pdf", "EmployeeReport.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpPost("employee-report/email")]
        public async Task<IActionResult> SendEmployeeReportEmailAsync([FromQuery] string email) 
        {
            try
            {
                await _employeeReportEmailService.SendEmployeeReportAsync(email);
                return Ok(new {message = "Report sended through email" });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error while While aceesing Service Layer");
                throw;
            }
        }
        [HttpPost("employee-report/whatsapp")]
        public async Task<IActionResult> SendEmployeeReportWhatsAppAsync([FromQuery] string phoneNumber)
        {
            try
            {
                await _notificationService.SendAsync(NotificationType.WhatsApp,phoneNumber,"Employee Report","Employee report has been generated successfully.");

                return Ok(new{message = "Employee report sent through WhatsApp"});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while sending employee report through WhatsApp");

                throw;
            }
        }
    }
}
