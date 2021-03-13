using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using _2020_backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using _2020_backend.Data;
using Microsoft.CodeAnalysis.Options;

namespace _2020_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if DEBUG
            Console.WriteLine("[Warning] Debug Version");
            Configuration["X-Real-IP"] = "10.10.10.10";
            Configuration["DB_HOST"] = "localhost";
            Configuration["DB_PORT"] = "8087";
            Configuration["DB_NAME"] = "naxin";
            Configuration["DB_USER"] = "a";
            Configuration["DB_PASSWORD"] = "a";
            Configuration["ADMIN_USERNAME"] = "zjueva";
            Configuration["ADMIN_PASSWORD"] = "a";
#endif
            if (Configuration["DB_PORT"] == string.Empty)
                Configuration["DB_PORT"] = "5432";
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                        });
                }).AddAuthentication(options =>
                {
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }
                )
                .AddCookie( options =>
                {
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                }
                );
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
                options.AddPolicy("ManagerAndHigher", policy => policy.RequireClaim(ClaimTypes.Role, "admin", "manager"));
            });
            services.AddRazorPages(options =>
            {
                options.Conventions.AllowAnonymousToFolder("/Account");
                options.Conventions.AllowAnonymousToPage("/api/submit");
                options.Conventions.AllowAnonymousToPage("/api/getinfo");
                options.Conventions.AuthorizeFolder("/Records");
                options.Conventions.AuthorizeFolder("/Result");
                options.Conventions.AuthorizeFolder("/Times");
                options.Conventions.AuthorizePage("/Times/Delete", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Times/Create", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Times/Edit", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Times/Reset", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Times/Resetperson", "ManagerAndHigher");

                options.Conventions.AuthorizePage("/Records/Delete", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Records/Edit", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Records/Fail", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Records/Pass", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Records/Pending", "ManagerAndHigher");
                options.Conventions.AuthorizePage("/Records/SMS", "ManagerAndHigher");
                options.Conventions.AuthorizeFolder("/Users", "ManagerAndHigher");
                options.Conventions.AuthorizeFolder("/Users/Edit", "Adminonly");
                options.Conventions.AuthorizeFolder("/Users/Create", "Adminonly");
                options.Conventions.AuthorizeFolder("/Users/Delete", "Adminonly");
                options.Conventions.AuthorizePage("/Migrate", "AdminOnly");
                options.Conventions.AllowAnonymousToPage("/Privacy");
            });

            services.AddDbContext<BackendContext>(options =>
                options.UseNpgsql($"Host={Configuration["DB_HOST"]};Port={Configuration["DB_PORT"]};Database={Configuration["DB_NAME"]};Username={Configuration["DB_USER"]};Password={Configuration["DB_PASSWORD"]}"));





        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
