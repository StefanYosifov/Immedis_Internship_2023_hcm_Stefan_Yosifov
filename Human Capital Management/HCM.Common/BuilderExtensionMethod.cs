namespace HCM.Common;

using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

public static class BuilderCustomExtensionMethods
{
    public static void RegisterApplicationServices(this IServiceCollection serviceCollection, Type serviceInterface)
    {
        var assembly = Assembly.GetAssembly(serviceInterface);

        var serviceClassType = assembly
            .GetTypes()
            .Where(s => s.Name.ToLower().EndsWith("service") && !s.IsInterface && !s.IsAbstract)
            .ToArray();

        foreach (var implementationType in serviceClassType)
        {
            var interfaceType = implementationType.GetInterface($"I{implementationType.Name}");

            serviceCollection.AddScoped(interfaceType, implementationType);
        }
    }

    public static IServiceCollection RegisterJwtAuthentication(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };
            });

        return serviceCollection;
    }
}