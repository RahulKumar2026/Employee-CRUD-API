namespace Employee_CRUD_API.Service.Interface
{
    public interface IGeneratePdfReport
    {
        Task<byte[]?> GeneratePdfReportAsync();
    }
}
