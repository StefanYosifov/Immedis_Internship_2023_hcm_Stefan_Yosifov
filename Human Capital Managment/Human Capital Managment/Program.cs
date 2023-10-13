namespace Human_Capital_Managment
{
    using Huamn_Capital_Management.Constants.Cookies;

    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.CookiePolicy;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
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
            
            builder.Services.AddAutoMapper(typeof(Program));

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

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}