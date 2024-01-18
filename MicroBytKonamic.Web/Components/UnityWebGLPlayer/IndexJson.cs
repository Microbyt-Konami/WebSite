namespace MicroBytKonamic.Web.Components.UnityWebGLPlayer.Json;

public partial class IndexJson
{
    public bool? showDiagnostics { get; set; }
    public string? loaderFilename { get; set; }
    public string? backGroundFilename { get; set; }
    public int? width { get; set; }
    public int? height { get; set; }
    public Config? config { get; set; }
}

public partial class Config
{
    public string? dataFilename { get; set; }
    public string? frameworkFilename { get; set; }
    public string? workerFilename { get; set; }
    public string? codeFilename { get; set; }
    public string? memoryFilename { get; set; }
    public string? symbolsFilename { get; set; }
    public string? companyName { get; set; }
    public string? productName { get; set; }
    public string? productVersion { get; set; }
}
