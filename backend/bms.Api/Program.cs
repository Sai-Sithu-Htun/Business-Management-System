using bms.Api.Data;
using bms.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using bms.Api.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<bms.Api.Data.BmsDbContext>(options =>
    options.UseMySql(
            connectionString!,
            new MySqlServerVersion(new Version(8,2,0)))
        )
;

builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ProjectMemberService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<PaymentMethodService>();
builder.Services.AddScoped<ProjectStatusService>();
builder.Services.AddScoped<TaskPriorityService>();
builder.Services.AddScoped<TaskStatusService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Add this block after: var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider .GetRequiredService<bms.Api.Data.BmsDbContext>();
    var hasher = new PasswordHasher<User>();

if (!context.Users.Any())
{
    var user = new User
    {
        UserName = "Test Admin",
        UserEmail = "admin@test.com",
        RoleId = 1,
        IsActive = true,
        CreatedAt = DateTime.Now
    };

    user.PasswordHash = hasher.HashPassword(user, "TestAdmin@123");

    context.Users.Add(user);
    context.SaveChanges();
}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
