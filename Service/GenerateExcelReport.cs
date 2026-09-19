using DocumentFormat.OpenXml.Drawing;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class GenerateExcelReport : IGenerateExcelReport
    {
        private readonly ILogger<GenerateExcelReport> _logger;
        private IExcelReportService _excelReportService;
        private IReportService _reportService;
        public GenerateExcelReport(ILogger<GenerateExcelReport> logger, IExcelReportService excelReportService, IReportService reportService) 
        {
            _logger = logger;
            _excelReportService = excelReportService;
            _reportService = reportService;
        }
        public async Task<byte[]?> GenerateExcelReportAsync() 
        {
            try
            {
                _logger.LogInformation("Starting employee Excel report generation");
                //calling IReportService
                var reponse = await _reportService.GetEmployeeReportAsync();
                //validation
                if (reponse == null) 
                {
                    _logger.LogInformation("Employee report data not found. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return null;
                }
                //caling genrate Excel service
                var excelFile = await _excelReportService.GenerateAsync(reponse);
                if (excelFile == null || excelFile.Length == 0)
                {
                    _logger.LogInformation("Excel report generation returned an empty file");
                    return null;
                }
                //returing result
                return excelFile;
            }
            catch (Exception ex) 
            {
                _logger.LogInformation("Internal Server error: 500");
                throw;
            }
        }
    }
}
