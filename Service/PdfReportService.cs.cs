using Employee_CRUD_API.DTOs;
using Employee_CRUD_API.Service.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;


namespace Employee_CRUD_API.Service
{
    public class PdfReportService : IPdfReportService
    {
        private readonly ILogger<PdfReportService> _logger;

        public PdfReportService(ILogger<PdfReportService> logger)
        {
            _logger = logger;
        }

        public async Task<byte[]?> GenerateAsync(EmployeeReportDto report)
        {
            try
            {
                if (report == null)
                {
                    _logger.LogInformation("Employee report data is null. Code: {Code}",HTTPResponseWrapper.Constants.NoDetailsFoundCode);

                    return null;
                }

                _logger.LogInformation("Creating PDF employee report");

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(40);

                        page.Header()
                            .Text("Employee Report")
                            .FontSize(20)
                            .Bold();

                        page.Content()
                            .PaddingVertical(20)
                            .Column(column =>
                            {
                                column.Spacing(10);

                                column.Item()
                                    .Text($"Total Employees: {report.TotalEmployees}")
                                    .FontSize(14);

                                column.Item()
                                    .Text($"Total Salary: {report.TotalSalary}")
                                    .FontSize(14);

                                column.Item()
                                    .Text($"Average Salary: {report.AverageSalary}")
                                    .FontSize(14);
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text("Employee CRUD API")
                            .FontSize(10);
                    });
                });

                byte[] pdfFile = document.GeneratePdf();

                _logger.LogInformation("PDF employee report generated successfully");

                return await Task.FromResult(pdfFile);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while generating PDF employee report");

                throw;
            }
        }
    }
}
