namespace Employee_CRUD_API.Service.Interface
{
    public interface IGenerateExcelReport
    {
        Task<byte[]?> GenerateExcelReportAsync();
    }
}
