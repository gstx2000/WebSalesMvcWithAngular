using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using Microsoft.EntityFrameworkCore.Design;
using WebSalesMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using SendGrid;
using Serilog;
using Serilog.Events;
using WebSalesMvcWithAngular.Data;

namespace WebSalesMvc
{
    public class Startup
    {
        public class CustomIdentityErrorDescriber : IdentityErrorDescriber
        {
            public override IdentityError DuplicateEmail(string Email)
            {
                return new IdentityError
                {
                    Code = nameof(DuplicateEmail),
                    Description = $"O Email {Email} já está em uso."
                };
            }
            public override IdentityError DuplicateUserName(string userName)
            {
                return new IdentityError
                {
                    Code = nameof(DuplicateUserName),
                    Description = string.Empty
                };
            }
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WebSalesMvcContext>
        {
            public WebSalesMvcContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var builder = new DbContextOptionsBuilder<WebSalesMvcContext>();
                var connectionString = configuration.GetConnectionString("ConnString");

                builder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 34)));



                return new WebSalesMvcContext(builder.Options);
            }
        }

        public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                var connectionString = configuration.GetConnectionString("ConnString");

                builder.UseMySql(connectionString,
                new MySqlServerVersion(new Version(8, 0, 34)));

                return new ApplicationDbContext(builder.Options);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllers();


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(3); // Set the desired expiration time
                options.LoginPath = "/Users/Login";
                options.LogoutPath = "/Users/Logout";
        
            });

        
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddConsole();
                builder.AddDebug();
                builder.AddSerilog();

            });



            services.AddDbContext<WebSalesMvcContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("ConnString");
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34))); 

            });

            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<PasswordRecoveryService>();
            services.AddScoped<ProductService>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("ConnString");
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)));

            });

            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddScoped<UserManager<User>>();
            services.AddScoped<SignInManager<User>>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var brl = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(brl),
                SupportedCultures = new List<CultureInfo> { brl },
                SupportedUICultures = new List<CultureInfo> { brl }
            };

            app.UseRequestLocalization(localizationOptions);

            app.UseCors("AllowAllOrigins");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            loggerFactory.AddSerilog(); 

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs\\WebSalesMvcWithAngular.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Users}/{action=Login}/{id?}");

                endpoints.MapControllerRoute(
                name: "Departments",
                pattern: "api/[controller]/{action=GetDepartments}/{id?}");
            });


        }
    }
}
