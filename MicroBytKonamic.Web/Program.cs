//using Microsoft.AspNetCore.ResponseCompression;
//using System.IO.Compression;

using MicroBytKonamic.Web.Components;
using MicroBytKonamic.Web.Services;
using Microsoft.AspNetCore.Localization;

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

builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");

builder.Services.AddSingleton<IResourcesServices, ResourcesServices>();
// MicrobytKonamic services
builder.Services.AddMicrobytKonamic();

// Add services to the container.
builder.Services.AddAntiforgery();
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddProblemDetails();

var app = builder.Build();

//app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");

app.UseHttpsRedirection();

var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();

provider.Mappings.Add(new KeyValuePair<string, string>(".data", "application/octet-stream"));

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});

var supportedCultures = new[] { "es", "en" };
var locOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en")
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
//locOptions.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(ctx =>
//{
//    var culture = ctx.Request.Cookies["lang"] ?? "en";
//    return Task.FromResult(new ProviderCultureResult(culture, culture))!;
//}));
app.UseRequestLocalization(locOptions);

app.UseRouting();
app.UseAntiforgery();

app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
