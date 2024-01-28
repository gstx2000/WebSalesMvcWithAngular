using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using WebSalesMvc.Data;
using Microsoft.EntityFrameworkCore.Design;
using WebSalesMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;
using WebSalesMvcWithAngular.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

/*-----------------------------------------------------------------------------------------------------------------------*/
                                        /*CORS, XSRF TOKEN AND COOKIE CONFIGURATION*/
/*-----------------------------------------------------------------------------------------------------------------------*/

builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<SalesRecordService>();
builder.Services.AddScoped<PasswordRecoveryService>();

builder.Services.AddCors(options => options.AddPolicy(
    name: "AllowAllOrigins",  
    policy =>
    {
        policy.WithOrigins("https://localhost:44493").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    }));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // For development; change to CookieSecurePolicy.Always in production
});

builder.Services.AddControllers();

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "XSRF-TOKEN";
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

/*-----------------------------------------------------------------------------------------------------------------------*/

/*
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(3); // Set the desired expiration time
    options.LoginPath = "/Users/Login";
    options.LogoutPath = "/Users/Logout";

});*/

/*-----------------------------------------------------------------------------------------------------------------------*/
                                       /*DB context and connection configuration*/
/*-----------------------------------------------------------------------------------------------------------------------*/

/*MODELS SCHEMA*/

builder.Services.AddDbContext<WebSalesMvcContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnString");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)));

});

/*USERS SCHEMA*/

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnString");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 34)));

});


/*-----------------------------------------------------------------------------------------------------------------------*/
                                                    /*Log configuration*/
/*-----------------------------------------------------------------------------------------------------------------------*/
builder.Services.AddLogging(builder =>
{
    builder.SetMinimumLevel(LogLevel.Debug);
    builder.AddConsole();
    builder.AddDebug();
    builder.AddSerilog();

});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()  // Set the minimum log level to Verbose for more detailed logging
    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)  // Override minimum level for Microsoft-related logs to Debug
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs\\WebSalesMvcWithAngular.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
/*-----------------------------------------------------------------------------------------------------------------------*/


var brl = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(brl),
    SupportedCultures = new List<CultureInfo> { brl },
};


/*builder.Services.AddDataProtection()
    .SetApplicationName("WebSalesMvcWithAngular")
    .PersistKeysToFileSystem((new DirectoryInfo(@"C:\Users\gst20\OneDrive\Área de Trabalho\SalesWebAngular\WebSalesMvcWithAngular\WebSalesMvcWithAngular/keys")));*/

/*builder.Services.AddIdentity<User, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<CustomIdentityErrorDescriber>();

builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});


builder.Services.Configure<IdentityOptions>(options =>
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
});*/


var app = builder.Build();

app.UseRouting();
app.UseAuthentication(); 
app.UseAuthorization();
app.UseCors("AllowAllOrigins");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Users}/{action=Login}/{id?}");
});



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseRequestLocalization(localizationOptions);

/*app.Use(nextDelegate => context =>
{
    var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
    string path = context.Request.Path.Value.ToLower();
    string[] directUrls = { "/create", "/cetDepartments" };
   
        AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions()
        {
            HttpOnly = false,
            Secure = false,
            IsEssential = true,
            SameSite = SameSiteMode.None
        });
    
    return nextDelegate(context);
});*/

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.Run();
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

