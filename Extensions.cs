using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;

namespace OnlyThrals
{
    public static class Extensions
    {
        private static Random rng = new Random();
        private static DateTime _nextShuffle;
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }
        public static string? QueryString(this NavigationManager navigationManager, string key)
        {
            key ??= string.Empty;
            return navigationManager.QueryString()[key];
        }
        public static void Shuffle<T>(this IList<T> list)
        {
            if (_nextShuffle >= DateTime.Now)
                return;

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            _nextShuffle = DateTime.Now.AddMinutes(5);
        }
    }
}
