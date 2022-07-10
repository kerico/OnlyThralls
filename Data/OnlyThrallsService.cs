using Microsoft.AspNetCore.Components;
using OnlyThrals.Model;
using TwitchIntegration;

namespace OnlyThrals.Data
{
    public class OnlyThrallsService
    {
        private IConfiguration _configuration;
        public static List<Thrall> _thralls = new()
        {
                new Thrall
                {
                    ID = 0,
                    Name = "Newt",
                    TwitchUserName = "newtalexander",
                    TwitterUserName = "NewtAlexander_",
                    Icon = "https://static-cdn.jtvnw.net/jtv_user_pictures/3e9dda88-5065-425e-ad11-0714aaa23b6a-profile_image-70x70.png",
                    Description = "Hi I'm Newt, I use he/they pronouns and I like playing games with my friends!",
                },
                new Thrall
                {
                    ID = 1,
                    Name = "Sean",
                    TwitchUserName = "imseanbean",
                    TwitterUserName = "ImSeanBean_",
                    Icon = "https://static-cdn.jtvnw.net/jtv_user_pictures/58d571ba-087e-4cb0-848d-1a9141724044-profile_image-70x70.png",
                    Description = "A true Yorkshire man making his way back home to get a cuppa tea.",
                },
                new Thrall
                {
                    ID = 2,
                    Name = "Jemalki",
                    TwitchUserName = "jemalki",
                    TwitterUserName = "jemalkiinlove",
                    Icon = "https://static-cdn.jtvnw.net/jtv_user_pictures/c982e0fe-c2f8-4090-b716-ec00537b2426-profile_image-70x70.png",
                    Description = "Hi I'm Jemal and I am the bass guitarist for Allusinlove. I go by many names The Sniper Lord, Shredder of Men, Cardinal of Funk, Blast Jockey, King of Snacks, Ol' Dusty, Friend.",
                },
                new Thrall
                {
                    ID = 3,
                    Name = "Rachel",
                    TwitchUserName = "rachelamy00",
                    TwitterUserName = "RachelAmy00",
                    Icon = "https://static-cdn.jtvnw.net/jtv_user_pictures/50f3d9ba-8be9-4e43-ab74-8c88e1fac31f-profile_image-70x70.png",
                    Description = "Hey I'm Rachel! Welcome to my channel! ♡",
                },
                new Thrall
                {
                    ID = 4,
                    Name = "Oprah",
                    TwitchUserName = "oprahswetcave",
                    TwitterUserName = "oprahswetcave",
                    Icon = "https://static-cdn.jtvnw.net/jtv_user_pictures/916fac4e-d15f-4ef9-8502-b6228c5fa355-profile_image-70x70.png",
                    Description = "OprahsWetCave streams Dread Hunger, Rust and Dead by Daylight.",
                },
                new Thrall
                {
                    ID = 5,
                    Name = "Pedguin",
                    TwitchUserName = "pedguin",
                    TwitterUserName = "pedguin",
                    Icon = "https://static-cdn.jtvnw.net/jtv_user_pictures/4098d6f6-18c0-42fe-ac65-d231a5ffaaa6-profile_image-70x70.png",
                    Description = "I love you",
                }
            };

        public OnlyThrallsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<List<Thrall>> GetThrallsForHomePageAsync()
        {
            _ = await GetThrallsAsync();

            var result = _thralls.Where(x => x.IsLive ?? false);

            if (result.Count() < 5)
            {
                var offlineThralls = _thralls.Where(x => !(x.IsLive ?? false)).Take(5 - result.Count());
                result = result.Union(offlineThralls);
            }

            return result.ToList();
        }
        public async Task<List<Thrall>> GetThrallsAsync()
        {
            _thralls.Shuffle();
            var twitchClient = new TwitchClient(_configuration["TwitchClientId"], _configuration["TwitchClientSecret"]);

            await _thralls.EnsureDetailsAsync(twitchClient);
            await _thralls.CheckIfOnlineAsync(twitchClient);
            return _thralls;

        }

        public static async Task UpdateThral(Thrall thrall)
        {
            await Task.FromResult(thrall.LastUpdated = DateTime.Now);
            //_thralls[thrall.ID] = thrall;
        }
    }
}
