namespace Employee_CRUD_API.Repository.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;

        public decimal Salary { get; set; }

        public DateTime? Created { get; set; }
    }
}
