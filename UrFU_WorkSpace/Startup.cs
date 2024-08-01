using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace.Repositories;
using UrFU_WorkSpace.Repositories.Interfaces;
using UrFU_WorkSpace.Services;
using UrFU_WorkSpace.Services.Interfaces;

namespace UrFU_WorkSpace;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        var baseAddress = Configuration.GetValue<string>("apiAddress");
        services.AddScoped<IReservationRepository, ReservationRepository>(sp => new ReservationRepository(baseAddress));
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>(sp => new WorkspaceRepository(baseAddress));
        services.AddScoped<IAmenityService, AmenityService>();

        services.AddScoped<IObjectService, ObjectService>();
        services.AddScoped<IOperationModeService, OperationModeService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IWorkspaceService, WorkspaceService>();
        services.AddScoped<AuthenticationService>(sp => new AuthenticationService(baseAddress));
        services.AddScoped<ReviewService>(sp => new ReviewService(new ReviewRepository(baseAddress)));

        services.AddHttpContextAccessor();
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Secret"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        services.AddSession();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceScopeFactory serviceScopeFactory)
    {
        app.UseSession();
        app.Use(async (context, next) =>
        {
            var JWToken = context.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(JWToken))
            {
                context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
            }

            await next();
        });
        app.UseAuthentication();
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}