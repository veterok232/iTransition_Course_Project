using Course_project.CloudStorage;
using Course_project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Course_project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserValidator<User>, UserValidator>();
            services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options => {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options => {
                    options.LoginPath = "/account/login";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = "1072569333629-vj3vuf1rl07fhres61nli5atq2r62h33.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddFacebook(options =>
                {
                    options.ClientId = "423598109219623";
                    options.ClientSecret = "4927a9b0633288b640dcf19ce063dbaa";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=HomeGuest}/{action=Index}/{id?}");
            });
        }
    }
}
