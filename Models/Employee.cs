namespace Employee_CRUD_API.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public decimal Salary { get; set; }

    public DateTime? Created { get; set; }
}
