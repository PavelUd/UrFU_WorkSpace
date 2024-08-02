using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UrFU_WorkSpace_API.Context;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace_API.Interfaces;
using UrFU_WorkSpace_API.Models;
using UrFU_WorkSpace_API.Repository;
using UrFU_WorkSpace_API.Repository.Interfaces;
using UrFU_WorkSpace_API.Services;
using UrFU_WorkSpace_API.Services.Interfaces;
using UrFU_WorkSpace_API.Services.WorkspaceComponentsServices;

namespace UrFU_WorkSpace_API;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddDbContext<UrfuWorkSpaceContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("Connection")));
        services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfiles()));
        services.AddSingleton(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<ErrorHandler>>();
            return new ErrorHandler(Configuration["ErrorsPath"], logger);
        });
        
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IWorkspaceProvider, WorkspaceRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>()
            .AddScoped<IBaseProvider<Review>, ReviewRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>()
            .AddScoped<IReservationProvider, ReservationRepository>();
        services.AddScoped<IBaseRepository<Template>, BaseRepository<Template>>()
            .AddScoped<IBaseProvider<Template>, BaseRepository<Template>>();
        services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
        services.AddScoped<IBaseRepository<Review>, BaseRepository<Review>>();
        services.AddScoped<IBaseRepository<Image>, BaseRepository<Image>>();
        services.AddScoped<IBaseRepository<WorkspaceAmenity>, BaseRepository<WorkspaceAmenity>>();
        services.AddScoped<IBaseRepository<WorkspaceObject>, BaseRepository<WorkspaceObject>>();
        services.AddScoped<IBaseRepository<WorkspaceObject>, BaseRepository<WorkspaceObject>>();
        services.AddScoped<IBaseRepository<WorkspaceWeekday>, BaseRepository<WorkspaceWeekday>>();
        
        services.AddScoped<ImageService>(provider =>
        {
            var repository = provider.GetRequiredService<IBaseRepository<Image>>();
            return new ImageService(repository, Configuration["HostName"], Configuration["ImagePath"]);
        });
        
        services.AddScoped<AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkspaceComponentService<WorkspaceAmenity>, WorkspaceAmenitiesService>();
        services.AddScoped<IWorkspaceObjectService, WorkspaceObjectsService>();
        services.AddScoped<IWorkspaceComponentService<WorkspaceWeekday>, OperationModeService>();
        services.AddScoped<IWorkspaceComponentService<WorkspaceWeekday>, OperationModeService>();
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ReservationService>();
        services.AddScoped<ReviewService>();
        services.AddScoped<IWorkspaceService, WorkspaceService>();
        services.AddScoped<TimeSlotsGenerator>();
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
        
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            var settings = options.SerializerSettings;
            settings.Converters.Add(new TimeOnlyJsonConverter());
            settings.Converters.Add(new DateOnlyJsonConverter());
            settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            settings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true
            });
        });
        services.AddSwaggerGenNewtonsoftSupport();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Co-Working API",
                Description = "API Сервиса бронирования коворкингов УрФУ"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
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
        if(!Directory.Exists(Configuration["ImagePath"]))
        {
            Directory.CreateDirectory(Configuration["ImagePath"]);
        }
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "images")),
            RequestPath = "/api/images"
        });

        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseMiddleware<JwtMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}