using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Web;

namespace MicroBytKonamic.Web.Components.Localization;

public class CultureInterativeUrlQuery : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        Uri uri = new Uri(NavigationManager.Uri);
        var parameters = HttpUtility.ParseQueryString(uri.Query);
        var culture = parameters["culture"];

        if (!string.IsNullOrEmpty(culture))
        {
            if (!string.Equals(CultureInfo.CurrentCulture.Name, culture, StringComparison.CurrentCultureIgnoreCase))
                CultureInfo.CurrentCulture = new CultureInfo(culture);
        }
    }
}
