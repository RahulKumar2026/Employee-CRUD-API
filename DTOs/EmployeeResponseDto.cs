namespace Employee_CRUD_API.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public decimal Salary { get; set; }
        public DateTime? Created { get; set; }

    }
}
