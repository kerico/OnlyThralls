using Microsoft.AspNetCore.Components;
using OnlyThrals.Model;
using TwitchIntegration;
using TwitchIntegration.Model;

namespace OnlyThrals.Data
{
    public static class DataExtensions
    {
        private static Dictionary<string, (bool isLive, DateTime nextCheck)> _liveThrallsCache = new Dictionary<string, (bool isLive, DateTime nextCheck)>();
        public static async Task EnsureDetailsAsync(this IEnumerable<Thrall> thralls, TwitchClient twitchClient)
        {
            var logins = thralls.Where(x => !x.LastUpdated.HasValue || (x.LastUpdated.HasValue && x.LastUpdated > DateTime.Now.AddMinutes(60))).Select(x => x.TwitchUserName ?? "");
            if (!logins.Any())
                return;

            var twitchUsers = (await twitchClient.GetUsersByLogin(logins?.ToArray() ?? throw new Exception("no thralls"))).ToLookup(x => x.Login, x => x);

            foreach (var thrall in thralls)
            {
                await thrall.UpdateFromTwitchAsync(twitchUsers[thrall.TwitchUserName?.ToLower() ?? throw new Exception("Twitch user name not specified")].FirstOrDefault());
            }

        }
        public static async Task UpdateFromTwitchAsync(this Thrall thrall, TwitchUserInfo? twitchUserInfo)
        {
            if (twitchUserInfo == null)
                return;

            //TODO: setup mapper
            if (!string.IsNullOrWhiteSpace(twitchUserInfo.Description))
                thrall.Description = twitchUserInfo.Description;
            thrall.Name = twitchUserInfo.DisplayName;
            thrall.Icon = twitchUserInfo.ProfileImageUrl;
            await OnlyThrallsService.UpdateThral(thrall);
        }

        public static async Task CheckIfOnlineAsync(this IEnumerable<Thrall> thralls, TwitchClient twitchClient)
        {
            var logins = thralls.Select(x => x.TwitchUserName ?? "");
            logins = logins.Where(x => !_liveThrallsCache.ContainsKey(x) || (_liveThrallsCache.ContainsKey(x) && _liveThrallsCache[x].nextCheck < DateTime.Now));
            if (!logins.Any())
                return;

            var streams = await twitchClient.GetStreamsByLogin(logins?.ToArray() ?? throw new Exception("no thralls"));

            foreach (var streamInfo in streams)
            {
                //TODO: check logic (too tired now)
                var thrall = thralls.First(x => x.TwitchUserName?.ToLower() == streamInfo.UserLogin);
                thrall.IsLive = true;
                if (!string.IsNullOrWhiteSpace(thrall.TwitchUserName))
                {
                    if (_liveThrallsCache.ContainsKey(thrall.TwitchUserName ?? throw new Exception("bad login")))
                    {
                        _liveThrallsCache[thrall.TwitchUserName] = new(true, DateTime.Now.AddMinutes(5));
                        continue;
                    }

                    _liveThrallsCache.Add(thrall.TwitchUserName, new(true, DateTime.Now.AddMinutes(5)));
                }
            }

            var offlineThralls = thralls.Where(x => !streams.Any(y => y.UserLogin == x.TwitchUserName));

            foreach (var thrall in offlineThralls)
            {
                thrall.IsLive = false;
                if (_liveThrallsCache.ContainsKey(thrall.TwitchUserName?? throw new Exception("bad login")))
                {
                    _liveThrallsCache[thrall.TwitchUserName] = new(false, DateTime.Now.AddMinutes(5));
                    continue;
                }

                _liveThrallsCache.Add(thrall.TwitchUserName, new(false, DateTime.Now.AddMinutes(5)));
            }
        }
    }
}
