namespace Human_Capital_Managment
{
    using Data;

    using Huamn_Capital_Management.Constants.Cookies;

    using Huamn_Capital_Managment.Common;

    using Human_Capital_Management.Services.Authentication;
    using Human_Capital_Management.Services.Home;
    using Human_Capital_Management.Services.UserDetails;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.CookiePolicy;
    using Microsoft.EntityFrameworkCore;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMvc();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(cookieOpt =>
                {
                    cookieOpt.Cookie.Name = CookieAuthenticationConstants.CookieName;
                    cookieOpt.LoginPath = CookieAuthenticationConstants.LoginPath;
                    cookieOpt.AccessDeniedPath = CookieAuthenticationConstants.AccessDeniedPath;
                });

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserDetailsService, UserDetailsService>();
            builder.Services.AddScoped<IHomeService, HomeService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.MapControllerRoute(
                name: "areas",
                pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );

            await app.RunAsync();
        }
    }
}