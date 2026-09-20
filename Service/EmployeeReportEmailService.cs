using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Enums;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class EmployeeReportEmailService : IEmployeeReportEmailService
    {
        private readonly IGenerateExcelReport _generateExcelReport;
        private readonly INotificationService _notificationService;
        private readonly ILogger<EmployeeReportEmailService> _logger;

        public EmployeeReportEmailService(IGenerateExcelReport generateExcelReport,INotificationService notificationService,ILogger<EmployeeReportEmailService> logger)
        {
            _generateExcelReport = generateExcelReport;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task SendEmployeeReportAsync(string email)
        {
            try
            {
                _logger.LogInformation("Generating employee Excel report for email");

                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogInformation("Email is required. Code: {Code}",HTTPResponseWrapper.Constants.BadRequestCode);

                    throw new ArgumentException("Email is required.",nameof(email));
                }
                var excelReport = await _generateExcelReport.GenerateExcelReportAsync();

                if (excelReport == null || excelReport.Length == 0)
                {
                    _logger.LogInformation("Employee Excel report was not generated");

                    return;
                }

                var attachment = new EmailAttachmentDto
                {
                    FileName = "EmployeeReport.xlsx",
                    FileData = excelReport,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };

                await _notificationService.SendAsync(NotificationType.Email,email, "employee Excel Report", "Generating employee Excel report for email.",
                    new List<EmailAttachmentDto>
                    {
                        attachment
                    });

                _logger.LogInformation("Employee report email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError( ex,"Error while generating and sending employee report");

                throw;
            }
        }
    }
}
