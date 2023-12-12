namespace MicroBytKonamic.Web.Services;

public class ResourcesServices : IResourcesServices
{
    private readonly IHostEnvironment _environment;
    private readonly string directoryRoot;

    public ResourcesServices(IWebHostEnvironment environment)
    {
        _environment = environment;
        directoryRoot = Path.GetDirectoryName(typeof(ResourcesServices).Assembly.FullName)!;
    }

    public string GetDirectory(string path) => Path.Combine(_environment.ContentRootPath, "Resources", path);

    public string GetRandomNavidadMp3()
    {
        var files = Directory.GetFiles(GetDirectory(@"Sounds/Navidad"), "*.mp3");
        var file = files[Random.Shared.Next(files.Length)];

        return file;
    }
}
