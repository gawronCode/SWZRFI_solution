using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWZRFI.BackgroundServices.Schedulers.Implementations;
using SWZRFI.BackgroundServices.Services.Abstract;
using SWZRFI.BackgroundServices.Services.Implementations;
using SWZRFI.ConfigData;
using SWZRFI.ConfigData.Locales;
using SWZRFI.ControllersServices.CompanyManager;
using SWZRFI.ControllersServices.JobOffers;
using SWZRFI.ControllersServices.JobOffersManager;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Implementations;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DAL.Utils;
using SWZRFI_Utils.EmailHelper;


namespace SWZRFI
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
            AddRepositories(services);
            AddContexts(services);
            SetIdentity(services);
            AddSingletonServices(services);
            AddScopedServices(services);


            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }


        private void AddSingletonServices(IServiceCollection services)
        {
            services.AddSingleton<IHostedService, AccountsCleanerScheduler>();
        }

        private void AddScopedServices(IServiceCollection services)
        {
            services.AddScoped<IBaseScheduledService, AccountsCleaner>();
            services.AddScoped<IDbConnectionChecker, DbConnectionChecker>();
            services.AddScoped<IConfigGetter, ConfigGetter>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IJobOffersService, JobOffersService>();
            services.AddScoped<IJobOffersManagerService, JobOffersManagerService>();
            services.AddScoped<ICompanyManagerService, CompanyManagerService>();
            
        }


        private void SetIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<UserAccount>(options =>
                    options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddErrorDescriber<PolishIdentityLocales>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        private void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IJobOfferRepo, JobOfferRepo>();
            services.AddScoped<ICompanyRepo, CompanyRepo>();
        }

        private void AddContexts(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SWZRFI")));
        }

        
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            UserManager<UserAccount> userManager,
            RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory serviceScopeFactory)
        {
            InitialDataSeed.Seed(userManager, roleManager, serviceScopeFactory).Wait();
            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
