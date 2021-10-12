using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models.IdentityModels;

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
            AddContext(services);
            AddIdentityAccounts(services);
            ConfigurePasswordRequirements(services);
            ConfigureMvc(services);

            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        private void AddIdentityAccounts(IServiceCollection services)
        {
            services.AddDefaultIdentity<PersonalAccount>(options =>
                options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ContextEf>();
            services.AddDefaultIdentity<CorporateAccount>(options =>
                options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ContextEf>();
        }

        private void ConfigurePasswordRequirements(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
        }

        private void AddContext(IServiceCollection services)
        {
            services.AddDbContext<ContextEf>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SWZRFI")));
        }

        
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            UserManager<PersonalAccount> userManager,
            RoleManager<IdentityRole> roleManager)
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
