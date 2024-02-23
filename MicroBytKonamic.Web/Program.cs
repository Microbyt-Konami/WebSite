//using Microsoft.AspNetCore.ResponseCompression;
//using System.IO.Compression;

using MicroBytKonamic.Commom.Services;
using MicroBytKonamic.Data.DataContext;
using MicroBytKonamic.Web.Components;
using MicroBytKonamic.Web.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

builder.Services
    .AddLocalization(opt => opt.ResourcesPath = "Resources")
    .AddSingleton<IResourcesServices, ResourcesServices>()
    .AddSingleton<LanguagesContainer>()
    //.AddMemoryCache()
    // MicrobytKonamic services
    .AddMicrobytKonamic()
// Add services to the container.
    .AddAntiforgery()
    .AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddProblemDetails().AddHttpClient("github", client => client.BaseAddress = new Uri("https://github.com/Microbyt-Konami/Navidad2023/raw/main/"));

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
provider.Mappings.Add(new KeyValuePair<string, string>(".bundle", "application/octet-stream"));

app.UseStaticFiles(new StaticFileOptions()
{
    ContentTypeProvider = provider
});

var languageContainer = app.Services.GetRequiredService<LanguagesContainer>();
//var supportedCultures = new[] { "es", "en" };
var locOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(languageContainer.DefaultLanguage)
    .AddSupportedCultures(languageContainer.SupportedLanguagesName)
    .AddSupportedUICultures(languageContainer.SupportedLanguagesName);

//locOptions.AddInitialRequestCultureProvider(new InterativeCultureProvider());
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


