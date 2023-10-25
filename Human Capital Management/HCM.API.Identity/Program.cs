using HCM.API.Services.Services.Countries;
using HCM.API.Services.Services.Department;
using HCM.API.Services.Services.Employee;
using HCM.API.Services.Services.File;
using HCM.API.Services.Services.Gender;
using HCM.API.Services.Services.Identity.Services;
using HCM.Common.AutoMapper;
using HCM.Common.Constants;
using HCM.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(ApplicationAPIConstants.MVC_BASE_URL)
                .WithMethods("GET","POST","PUT","DELETE");
        });
});

builder.Services.Configure<FormOptions>(opt =>
{
    opt.ValueCountLimit = int.MaxValue; 
    opt.ValueLengthLimit = 1024 * 1024 * 100;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
