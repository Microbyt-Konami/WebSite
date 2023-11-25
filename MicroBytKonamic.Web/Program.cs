//using Microsoft.AspNetCore.ResponseCompression;
//using System.IO.Compression;

using Microsoft.AspNetCore.StaticFiles;

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

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
