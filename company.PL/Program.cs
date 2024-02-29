using company.BLL.interfaces;
using company.BLL.Repositrios;
using company.PL.Helper;
using company.PL.MapperProfiel;
using company.PL.Settings;
using Company.DAL.Context;
using Company.DAL.models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

 
            #region Configure Services thats Allow Dependancy injaction
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<CompanyContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });//one object(application)
               ////  services.AddTransient<IDepartmentRepository, DepartmentRepository>(); //per operation
               // services.AddScoped<IDepartmentRepository, DepartmentRepository>();//per request
               //  //services.AddSingleton<IDepartmentRepository, DepartmentRepository>();//per session (application) //Caching //Logger //signalR
               //  services.AddScoped<IemployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(M => M.AddProfile(new RoleProfile()));

            builder.Services.AddAutoMapper(D => D.AddProfile(new DepartmentProfile()));
            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<CompanyContext>()
            .AddDefaultTokenProviders(); //Default Tokens


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "Account/Login";
                    options.AccessDeniedPath = "Home/Error";


                });

            //MailKit

            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
            builder.Services.AddTransient<IMailSetting,EmailSetting>();

            //Twilio
            builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("Twilio"));
            builder.Services.AddTransient<ISmsMessage,SmsSetting>();

            //google
            //builder.Services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //}).AddGoogle(o =>
            //{
            //    IConfiguration GoogleAuthSection = builder.Configuration.GetSection("Authentication:Google");
            //    o.ClientId = GoogleAuthSection["ClientId"];
            //    o.ClientSecret = GoogleAuthSection["ClientSecret"];
            //});

            #endregion



            var app=builder.Build();
            var env = builder.Environment;
            #region Configure Http Request pipline
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            #endregion


            app.Run();
        }

       
    }
}
