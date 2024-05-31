using System.Text.Json.Serialization;
using UrFU_WorkSpace_API.Helpers;
using UrFU_WorkSpace.Helpers;
using UrFU_WorkSpace.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddScoped<ReviewRepository>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var baseAddress = configuration.GetValue<string>("apiAddress");
    return new ReviewRepository(baseAddress);
});
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