using Microsoft.EntityFrameworkCore;
using AdeiesApplication.Domain;
using AdeiesApplication.Persistance;
using AdeiesApplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
            //options.AccessDeniedPath = "/api/Auth/AccessDenied";
            options.LogoutPath = "/api/Auth/Logout";
            options.LoginPath = "/api/Auth/Login";
            
        });

        //add localization
        

      
        
        builder.Services.AddLocalization(opt=>opt.ResourcesPath = "Resources");
        builder.Services.Configure<RequestLocalizationOptions>(opt =>{
            var supportedCultures = new[]
            {
                new CultureInfo ("en-US"),
                new CultureInfo ("gr-GR")
            };

            opt.SupportedUICultures = supportedCultures;
            opt.SupportedUICultures = supportedCultures;
            opt.SetDefaultCulture("en-US");
            opt.ApplyCurrentCultureToResponseHeaders = true;
            //opt.RequestCultureProviders = new List<RequestCultureProvider>() {
             
            //};

        });

        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<RegistrationRequest>, RequestRepository>();
        builder.Services.AddScoped<IRepository<Vacation>, VacationRepository>();
        builder.Services.AddScoped<PasswordHasher<User>>();

        //http
        //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //builder.Services.AddHttpContextAccessor();

        builder.Services.AddSwaggerGen();

        var app = builder.Build();


       var opts= app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value;
        app.UseRequestLocalization(opts);    

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}