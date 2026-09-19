using ClosedXML.Excel;
using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Service.Interface;

namespace Employee_CRUD_API.Service
{
    public class ExcelReportService : IExcelReportService
    {
        private readonly ILogger<ExcelReportService> _logger;

        public ExcelReportService(ILogger<ExcelReportService> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]> GenerateAsync(EmployeeReportDto report)
        {
            try
            {
                //validating
                if (report == null)
                {
                    _logger.LogInformation("Employee data not found.Code: { Code}", HTTPResponseWrapper.Constants.NoDetailsFoundCode);
                    return null;
                }
                _logger.LogInformation("Generating Excel employee report");

                using var workbook = new XLWorkbook();

                var worksheet = workbook.Worksheets.Add("Employee Report");

                // Header
                worksheet.Cell(1, 1).Value = "Employee Report";
                worksheet.Cell(3, 1).Value = "Total Employees";
                worksheet.Cell(3, 2).Value = report.TotalEmployees;

                worksheet.Cell(4, 1).Value = "Total Salary";
                worksheet.Cell(4, 2).Value = report.TotalSalary;

                worksheet.Cell(5, 1).Value = "Average Salary";
                worksheet.Cell(5, 2).Value = report.AverageSalary;

                // Formatting
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(3, 1).Style.Font.Bold = true;
                worksheet.Cell(4, 1).Style.Font.Bold = true;
                worksheet.Cell(5, 1).Style.Font.Bold = true;

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                _logger.LogInformation("Excel employee report generated successfully");

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while generating Excel employee report");

                throw;
            }
        }
    }
}