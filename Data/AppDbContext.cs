using DocumentFormat.OpenXml.Spreadsheet;
using Employee_CRUD_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_CRUD_API.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Department> Departments { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=Rahul@pratap05");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("Departments_pkey");

            entity.Property(e => e.DepartmentName).HasMaxLength(100);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("Employees_pkey");

            entity.Property(e => e.Created)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created");
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.Salary).HasPrecision(10, 2);
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId)
                .HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id");

            entity.Property(e => e.Username)
                .HasColumnName("username")
                .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .HasMaxLength(500);

            entity.Property(e => e.Role)
                .HasColumnName("role")
                .HasMaxLength(50);

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

            entity.Property(e => e.CreatedOn)
                .HasColumnName("created_on")
                .HasColumnType("timestamp without time zone");
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
