using Employee_CRUD_API.Data;
using Employee_CRUD_API.Helper;
using Employee_CRUD_API.MiddleWares;
using Employee_CRUD_API.Repository;
using Employee_CRUD_API.Repository.Interface;
using Employee_CRUD_API.Service;
using Employee_CRUD_API.Service.Interface;
using Employee_CRUD_API.Settings;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Serilog;

//Logger 

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/employee-api-.log",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

var smtpSettings = builder.Configuration
    .GetSection("SmtpSettings")
    .Get<SmtpSettings>();


// QuestPDF license
QuestPDF.Settings.License = LicenseType.Community;

//Adding Serilog
builder.Host.UseSerilog();

// PostgreSQL + EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Dependency Injection
builder.Services.AddScoped<Sorting>();

builder.Services.AddScoped<ISalaryCalculateService, SalaryCalculateService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ISalaryUpdateService, SalaryUpdateService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IExcelReportService, ExcelReportService>();
builder.Services.AddScoped<IGenerateExcelReport, GenerateExcelReport>();
builder.Services.AddScoped<IPdfReportService, PdfReportService>();
builder.Services.AddScoped<IGeneratePdfReport, GeneratePdfReport>();
builder.Services.AddScoped<INotificationService,NotificationService>();
builder.Services.AddScoped<INotificationProvider, WhatsAppNotificationProvider>();
builder.Services.AddScoped<INotificationProvider,EmailNotificationProvider>();
builder.Services.AddScoped< IEmployeeReportEmailService,EmployeeReportEmailService>();
builder.Services.AddSingleton(smtpSettings!);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();