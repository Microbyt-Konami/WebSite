using Markdig;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace System.Net.Http;

public static class HttpClientExtensions
{
    private static readonly Lazy<Encoding> _iso88591 = new Lazy<Encoding>(() => Encoding.GetEncoding("ISO-8859-1"));

    public async static Task<string> GetStringAsync(this HttpClient client, string url, Encoding encoding, CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync(url, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        response.EnsureSuccessStatusCode();

        byte[] resultBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        string text = Encoding.GetEncoding("ISO-8859-1").GetString(resultBytes);

        return text;
    }

    public async static Task<string> GetStringFromGitHubRawAsync(this HttpClient http, string url, CancellationToken cancellationToken = default) => await http.GetStringAsync(url, _iso88591.Value, cancellationToken);

    public async static Task<string> GetMarkdownFromGitHubRawAsync(this HttpClient http, string url, CancellationToken cancellationToken = default)
    {
        var text = await http.GetStringFromGitHubRawAsync(url, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        var markDown = Markdown.ToHtml(text);

        return markDown;
    }
}
