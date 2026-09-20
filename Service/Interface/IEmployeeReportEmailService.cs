namespace Employee_CRUD_API.Service.Interface
{
    public interface IEmployeeReportEmailService
    {
         Task SendEmployeeReportAsync(string email);
    }
}
