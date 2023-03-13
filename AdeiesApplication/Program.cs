using Microsoft.EntityFrameworkCore;
using AdeiesApplication.Domain;
using AdeiesApplication.Persistance;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

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
        });

        builder.Services.AddAuthorization();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<RequestRepository>();
        builder.Services.AddScoped<VacationRepository>();
        builder.Services.AddScoped<PasswordHasher<UserCreate>>();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Swagger Api",
                Description = "A simple example ASP.NET Core Web Api",
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

		// In case we want to use a secure proxy
		//app.UseForwardedHeaders(new ForwardedHeadersOptions
		//{
		//	ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
		//});

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
			// In case we use HTTPS
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			//app.UseHsts();
        }
        else
        {
			app.UseDeveloperExceptionPage();
        }

        // Swagger is needed for now and since we run a Release configuration
        // for now we will place it here
        app.UseSwagger(options =>
        {
            options.SerializeAsV2 = true;
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //c.RoutePrefix = string.Empty;

        });

        // In case we use HTTPS
        //app.UseHttpsRedirection();
        app.UseStaticFiles();


        app.UseRouting();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>().Value);    
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}