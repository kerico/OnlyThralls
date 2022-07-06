using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;

namespace OnlyThrals
{
    public static class Extensions
    {
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }
        public static string? QueryString(this NavigationManager navigationManager, string key)
        {
            key ??= string.Empty;
            return navigationManager.QueryString()[key];
        }
    }
}
