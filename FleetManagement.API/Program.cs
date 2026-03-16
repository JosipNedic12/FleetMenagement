using System.Text;
using FleetManagement.Application.Interfaces;
using FleetManagement.Infrastructure.Data;
using FleetManagement.Infrastructure.Repositories;
using FleetManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- Database ---
builder.Services.AddDbContext<FleetDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorCodesToAdd: null)));

// --- Repositories ---
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IVehicleAssignmentRepository, VehicleAssignmentRepository>();

// --- JWT Service ---
builder.Services.AddScoped<IJwtService, JwtService>();

// --- Authentication ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization();

// --- FluentValidation ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// --- Controllers ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- Swagger with JWT support ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fleet Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGci..."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddScoped<IOdometerLogRepository, OdometerLogRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IMaintenanceOrderRepository, MaintenanceOrderRepository>();
builder.Services.AddScoped<IFuelCardRepository, FuelCardRepository>();
builder.Services.AddScoped<IFuelTransactionRepository, FuelTransactionRepository>();
builder.Services.AddScoped<IInsurancePolicyRepository, InsurancePolicyRepository>();
builder.Services.AddScoped<IRegistrationRecordRepository, RegistrationRecordRepository>();
builder.Services.AddScoped<IFineRepository, FineRepository>();
builder.Services.AddScoped<IAccidentRepository, AccidentRepository>();
builder.Services.AddScoped<IInspectionRepository, InspectionRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddMemoryCache();
var app = builder.Build();

// Ensure uploads folder exists
Directory.CreateDirectory(app.Configuration["FileStorage:UploadPath"] ?? "uploads");

// Seed sample notifications for userId=1
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FleetManagement.Infrastructure.Data.FleetDbContext>();
    if (!db.Notifications.Any(n => n.UserId == 1))
    {
        var now = DateTime.UtcNow;
        db.Notifications.AddRange(
            new FleetManagement.Domain.Entities.Notification
            {
                UserId = 1, Type = "danger", IsRead = false, CreatedAt = now.AddMinutes(-5),
                Title = "Insurance Expired",
                Message = "Vehicle BA-123-AB insurance policy expired 3 days ago. Renew immediately.",
                RelatedEntityType = "insurance", RelatedEntityId = 1
            },
            new FleetManagement.Domain.Entities.Notification
            {
                UserId = 1, Type = "warning", IsRead = false, CreatedAt = now.AddHours(-2),
                Title = "Inspection Due Soon",
                Message = "Vehicle ZG-456-CD is due for technical inspection in 7 days.",
                RelatedEntityType = "inspection", RelatedEntityId = 2
            },
            new FleetManagement.Domain.Entities.Notification
            {
                UserId = 1, Type = "warning", IsRead = false, CreatedAt = now.AddHours(-5),
                Title = "Overdue Maintenance Order",
                Message = "Work order #14 (Oil change – ZG-789-EF) is overdue by 2 days.",
                RelatedEntityType = "maintenance", RelatedEntityId = 14
            },
            new FleetManagement.Domain.Entities.Notification
            {
                UserId = 1, Type = "info", IsRead = true, CreatedAt = now.AddDays(-1),
                Title = "New Driver Assigned",
                Message = "Marko Horvat has been assigned to vehicle BA-321-GH.",
                RelatedEntityType = "vehicle", RelatedEntityId = 3
            },
            new FleetManagement.Domain.Entities.Notification
            {
                UserId = 1, Type = "success", IsRead = true, CreatedAt = now.AddDays(-2),
                Title = "Maintenance Completed",
                Message = "Work order #11 (Brake service – ZG-654-IJ) was closed successfully.",
                RelatedEntityType = "maintenance", RelatedEntityId = 11
            },
            new FleetManagement.Domain.Entities.Notification
            {
                UserId = 1, Type = "danger", IsRead = false, CreatedAt = now.AddDays(-3),
                Title = "Unpaid Fine",
                Message = "Vehicle BA-999-KL has an unpaid fine of 150 EUR from 3 days ago.",
                RelatedEntityType = "fine", RelatedEntityId = 5
            }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // must be before UseAuthorization
app.UseAuthorization();
app.MapControllers();
app.Run();