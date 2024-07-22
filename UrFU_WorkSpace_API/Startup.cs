using System.Net.Mime;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Repository;
using UrFU_WorkSpace_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

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
        services.AddMemoryCache();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IAmenityTemplateRepository, AmenityTemplateRepository>();
        services.AddScoped<IBaseRepository<AmenityTemplate>, AmenityTemplateRepository>();
        services.AddScoped<IBaseRepository<Image>, BaseRepository<Image>>();
        services.AddScoped<IBaseRepository<ObjectTemplate>, ObjectTemplateRepository>();
        services.AddScoped<IBaseRepository<WorkspaceAmenity>, WorkspaceAmenityRepository>();
        services.AddScoped<IBaseRepository<WorkspaceObject>, WorkspaceObjectRepository>();
        services.AddScoped<IBaseRepository<WorkspaceWeekday>, OperationModeRepository>();
        services.AddScoped<IObjectTemplateRepository, ObjectTemplateRepository>();
        services.AddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        services.AddScoped<ImageService>();
        services.AddScoped<IWorkspaceComponentService<WorkspaceAmenity>, WorkspaceAmenitiesService>();
        services.AddScoped<IWorkspaceComponentService<WorkspaceObject>, WorkspaceObjectsService>();
        services.AddScoped<IWorkspaceComponentService<WorkspaceWeekday>, OperationModeService>();
        services.AddScoped(typeof(TemplateService<AmenityTemplate>));
        services.AddScoped(typeof(TemplateService<ObjectTemplate>));
        services.AddScoped<IWorkspaceComponentService<WorkspaceWeekday>, OperationModeService>();
        services.AddScoped<WorkspaceService>();
        services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfiles(services.BuildServiceProvider().GetService<IUserRepository>(), services.BuildServiceProvider().GetService<IWorkspaceRepository>())));
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
        
        
        
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            var settings = options.SerializerSettings;
            settings.Converters.Add(new TimeOnlyJsonConverter());
            settings.Converters.Add(new DateOnlyJsonConverter());
            settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
        });

        
        services.AddDbContext<UrfuWorkSpaceContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("Connection")));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Co-Working API",
                Description = "API Сервиса бронирования коворкингов УрФУ",
            });
            
            options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Введите строку авторизации следующим образом: `Bearer созданный JWT Токен`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
        
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseStaticFiles(); 
        
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "Images")),
            RequestPath = "/api/images"
        });
        
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<JwtMiddleware>();
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