using HCM.Common;
using HCM.Common.AutoMapper;
using HCM.Common.Constants;
using HCM.Common.Manager;
using HCM.Core.Services.Countries;
using HCM.Core.Services.Department;
using HCM.Core.Services.Employee;
using HCM.Core.Services.Gender;
using HCM.Core.Services.Identity;
using HCM.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IEmployeeManager, EmployeeManager>();

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

builder.Services.RegisterJwtAuthentication(builder.Configuration);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
