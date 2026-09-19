using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class GeneratePdfReport : IGeneratePdfReport
    {
        private readonly ILogger<GeneratePdfReport> _logger;
        private readonly IPdfReportService _pdfReportService;
        private readonly IReportService _reportService;

        public GeneratePdfReport(ILogger<GeneratePdfReport> logger,IPdfReportService pdfReportService,IReportService reportService)
        {
            _logger = logger;
            _pdfReportService = pdfReportService;
            _reportService = reportService;
        }

        public async Task<byte[]?> GeneratePdfReportAsync()
        {
            try
            {
                _logger.LogInformation("Starting employee PDF report generation");

                // Get employee report data
                var report = await _reportService.GetEmployeeReportAsync();

                // Validate report
                if (report == null)
                {
                    _logger.LogInformation("Employee report data not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);

                    return null;
                }

                // Generate PDF
                var pdfFile = await _pdfReportService.GenerateAsync(report);

                if (pdfFile == null || pdfFile.Length == 0)
                {
                    _logger.LogInformation("PDF report generation returned an empty file");

                    return null;
                }

                _logger.LogInformation("Employee PDF report generated successfully");

                return pdfFile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while generating employee PDF report");

                throw;
            }
        }
    }
}