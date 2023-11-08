using HCM.Common;
using HCM.Common.AutoMapper;
using HCM.Common.Manager;
using HCM.Core.Services.Identity;

using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.RegisterApplicationServices(typeof(IdentityService));
builder.Services.AddScoped<IEmployeeManager, EmployeeManager>();

builder.Services.RegisterDatabase(builder.Configuration)
    .RegisterJwtAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:7039")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.Configure<FormOptions>(opt =>
{
    opt.ValueCountLimit = int.MaxValue;
    opt.ValueLengthLimit = 104_857_600;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<AuditMiddleWare>();

app.MapControllers();

await app.RunAsync();