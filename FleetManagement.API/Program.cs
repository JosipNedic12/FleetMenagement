using FleetManagement.Application.Interfaces;
using FleetManagement.Infrastructure.Data;
using FleetManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FleetDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IVehicleAssignmentRepository, VehicleAssignmentRepository>(); // NEW

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/test-db", async (FleetDbContext db) =>
{
    var makes = await db.VehicleMakes.Take(5).ToListAsync();
    return Results.Ok(makes);
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();