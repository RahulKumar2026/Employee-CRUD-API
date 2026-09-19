namespace Employee_CRUD_API.Repository.DTOs
{
    public class EmployeeCreateDto
    {
        public string EmployeeName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
    }
}
