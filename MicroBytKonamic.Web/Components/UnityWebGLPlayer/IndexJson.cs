namespace MicroBytKonamic.Web.Components.UnityWebGLPlayer;

public class IndexJson
{
    public bool? showDiagnostics { get; set; }
    public bool? backGroundFilename { get; set; }
    public int? width { get; set; }
    public int? height { get; set; }
    public Config? config { get; set; }
}

public class Config
{
    public string? dataFilename { get; set; }
    public string? frameworkFilename { get; set; }
    public string? workerFilename { get; set; }
    public string? codeFilename { get; set; }
    public string? memoryUrl { get; set; }
    public string? symbolsFilename { get; set; }
    public string? companyName { get; set; }
    public string? productName { get; set; }
    public string? productVersion { get; set; }
}
