using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Repository;
using UrFU_WorkSpace_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;

namespace UrFU_WorkSpace_API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfiles(services.BuildServiceProvider().GetService<IWorkspaceRepository>())));
        services.AddAuthentication(x =>
        {
            
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Secret"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
        
        
        
        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        
        services.AddDbContext<UrfuWorkSpaceContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("Connection")));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
    
}