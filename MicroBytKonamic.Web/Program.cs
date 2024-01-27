//using Microsoft.AspNetCore.ResponseCompression;
//using System.IO.Compression;

using MicroBytKonamic.Web.Components;
using MicroBytKonamic.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Text.RegularExpressions;

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
locOptions.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(ctx =>
{
    var request = ctx.Request;

    if (request.Path.StartsWithSegments("/_blazor"))
    {
        //return Task.FromResult(new ProviderCultureResult("en"))!;
        var cookie = ctx.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

        if (cookie == null)
            return Task.FromResult((ProviderCultureResult?)null);

        var providerResultCulture = CookieRequestCultureProvider.ParseCookieValue(cookie);

        return Task.FromResult<ProviderCultureResult?>(providerResultCulture);
    }

    string? culture;
    string? UIculture;
    var queryCulture = request.Query["culture"];

    if (!string.IsNullOrWhiteSpace(queryCulture))
        culture = queryCulture;
    else
    {
        var queryUICulture = request.Query["ui-culture"];

        if (!string.IsNullOrWhiteSpace(queryUICulture))
            culture = queryUICulture;
        else
        {
            var acceptLanguageHeader = request.GetTypedHeaders().AcceptLanguage;
            culture=
        }
    }

    if (culture!=null)
    {

    }

    return Task.FromResult((ProviderCultureResult?)null);
}));
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
