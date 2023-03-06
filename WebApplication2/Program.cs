using Microsoft.EntityFrameworkCore;
using WebApplication2.Domain;
using WebApplication2.Persistance;
using WebApplication2.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


;
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
            });

        //add localization
        //builder.Services.AddLocalization();

        //var localizationOptions = new RequestLocalizationOptions();

        //var supportedCultures = new[]
        //{
        //    new CultureInfo ("en-US"),
        //    new CultureInfo ("gr-GR")
        //};

        //localizationOptions.SupportedUICultures = supportedCultures;
        //localizationOptions.SupportedUICultures = supportedCultures;
        //localizationOptions.SetDefaultCulture("en-US");
        //localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

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

        //app.UseRequestLocalization(localizationOptions);    

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