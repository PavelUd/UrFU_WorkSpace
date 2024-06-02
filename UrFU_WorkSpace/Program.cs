using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Services;
using UrFU_WorkSpace.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var baseAddress = builder.Configuration.GetValue<string>("apiAddress");
builder.Services.AddScoped<ReviewRepository>(sp => new ReviewRepository(baseAddress));
builder.Services.AddScoped<IReservationRepository, ReservationRepository>(sp => new ReservationRepository(baseAddress));
builder.Services.AddScoped<IObjectRepository, ObjectRepository>(sp => new ObjectRepository(baseAddress));
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>(sp => new WorkspaceRepository(baseAddress));
builder.Services.AddScoped<IOperationModeRepository, OperationModeRepository>(sp => new OperationModeRepository(baseAddress));
builder.Services.AddScoped<IImageRepository,ImageRepository>(sp => new ImageRepository(baseAddress));

builder.Services.AddScoped<IObjectService, ObjectService>();
builder.Services.AddScoped<IOperationModeService, OperationModeService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IWorkspaceService, WorkspaceService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();