using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SWZRFI.ConfigData;
using SWZRFI.ConfigData.Locales;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models.IdentityModels;
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
            AddContexts(services);
            SetIdentity(services);
            AddScopedServices(services);


            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        

        private void AddScopedServices(IServiceCollection services)
        {
            services.AddScoped<IConfigGetter, ConfigGetter>();
            services.AddScoped<IEmailSender, EmailSender>();
        }


        private void SetIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<UserAccount>(options =>
                    options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddErrorDescriber<PolishIdentityLocales>()
                .AddEntityFrameworkStores<ContextAccounts>();

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

        private void AddContexts(IServiceCollection services)
        {
            services.AddDbContext<ContextAccounts>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SWZRFI")));
        }

        
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            
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
