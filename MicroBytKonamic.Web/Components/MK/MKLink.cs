using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Web;

namespace MicroBytKonamic.Web.Components.MK;

public class MKLink : ComponentBase
{
    private string? _href;

    [Parameter] public string? Href { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be added to the generated
    /// <c>a</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value representing the URL matching behavior.
    /// </summary>
    [Parameter] public NavLinkMatch Match { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private ILogger<MKLink> Log { get; set; } = default!;

    protected override void OnParametersSet()
    {
        _href = Href;
        if (Href != null)
        {
            try
            {
                Uri uri = new Uri(NavigationManager.Uri);
                var parameters = HttpUtility.ParseQueryString(uri.Query);
                var culture = parameters["culture"];

                if (!string.IsNullOrEmpty(culture))
                {
                    /*
                    var _uri = NavigationManager.ToAbsoluteUri(Href);

                    //if (!_uri.StartsWith("/"))
                    //    _uri = $"/{_uri}";

                    var hrefUriBuilder = new UriBuilder(_uri);

                    if (!string.IsNullOrEmpty(hrefUriBuilder.Query))
                        hrefUriBuilder.Query += $"&";
                    else
                        hrefUriBuilder.Query = $"?";
                    hrefUriBuilder.Query += $"culture={culture}";
                    _href = NavigationManager.ToBaseRelativePath(hrefUriBuilder.ToString());
                    */
                    _href = NavigationManager.GetUriWithQueryParameters(Href, new Dictionary<string, object?> { ["culture"] = culture });
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex, $"href: '{_href}' incorrect");
            }
            //NavigationManager.

        }
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "a");

        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "href", _href);
        builder.AddContent(4, ChildContent);

        builder.CloseElement();
    }
}
