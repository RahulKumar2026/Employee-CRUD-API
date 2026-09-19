using Employee_CRUD_API.DTOs;

namespace Employee_CRUD_API.Service.Interface
{
    public interface IPdfReportService
    {
        Task<byte[]?> GenerateAsync(EmployeeReportDto report);
    }
}
