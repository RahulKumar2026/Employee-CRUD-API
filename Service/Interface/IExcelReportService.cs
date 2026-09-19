using Employee_CRUD_API.DTOs;

namespace Employee_CRUD_API.Service.Interface
{
    public interface IExcelReportService
    {
        Task<byte[]> GenerateAsync(EmployeeReportDto report);
    }
}
