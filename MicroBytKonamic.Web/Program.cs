// USE_MVC
// USE_BLAZORAPP

//using Microsoft.AspNetCore.ResponseCompression;
//using System.IO.Compression;

using MicroBytKonamic.Web.Components;
using MicroBytKonamic.Web.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddResponseCompression(options =>
//{
//    options.EnableForHttps = true;
//    options.Providers.Add<GzipCompressionProvider>();
//});

//builder.Services.Configure<GzipCompressionProviderOptions>(options =>
//{
//    options.Level = CompressionLevel.SmallestSize;
//});

builder.Services.AddSingleton<IResourcesServices, ResourcesServices>();
// MicrobytKonamic services
builder.Services.AddMicrobytKonamic();

// Add services to the container.
#if USE_MVC
builder.Services.AddControllersWithViews();
#elif USE_BLAZORAPP
builder.Services.AddAntiforgery();
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
#endif
builder.Services.AddProblemDetails();

var app = builder.Build();

//app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

provider.Mappings.Add(new KeyValuePair<string, string>(".data", "application/octet-stream"));

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});
app.UseRouting();
app.UseAntiforgery();

#if USE_MVC
#endif

app.UseAuthorization();

#if USE_MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
#elif USE_BLAZORAPP
app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
#endif

app.Run();
