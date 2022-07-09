using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitchIntegration.Model;

namespace TwitchIntegration
{
    public class TwitchClient
    {
        private TwitchAccessToken? _accessToken;
        private DateTime? _accessTokenExpiration;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _grantType;
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://api.twitch.tv/helix/")
        };
        private TwitchAccessToken AccessToken
        {
            get
            {
                GetAccessToken();

                return _accessToken ?? throw new Exception("Failed to get access token");
            }
        }
        public TwitchClient(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _grantType = "client_credentials";
        }
        public async Task<TwitchStreamInfo[]> GetStreamsByLogin(params string[] streamLogins)
        {
            //var request = GetHttpRequestMessage("streams", HttpMethod.Get);
            var request = GetHttpRequestMessage(AddParameters("streams", "user_login", streamLogins), HttpMethod.Get);
            var resposne = await _httpClient.SendAsync(request);
            resposne.EnsureSuccessStatusCode();
            var content = await resposne.Content.ReadAsAsync<TwitchResponse<TwitchStreamInfo>>();
            return content?.Values ?? Array.Empty<TwitchStreamInfo>();
        }
        public async Task<TwitchUserInfo[]> GetUsersByLogin(params string[] streamLogins)
        {
            //var request = GetHttpRequestMessage("streams", HttpMethod.Get);
            var request = GetHttpRequestMessage(AddParameters("users", "login", streamLogins), HttpMethod.Get);
            var resposne = await _httpClient.SendAsync(request);
            resposne.EnsureSuccessStatusCode();
            var content = await resposne.Content.ReadAsAsync<TwitchResponse<TwitchUserInfo>>();
            return content?.Values ?? Array.Empty<TwitchUserInfo>();
        }

        private static string AddParameters(string originalPath,string parametername, string[] logins)
        {
            var result = originalPath;
            for (int i = 0; i < logins.Length; i++)
            {
                result = QueryHelpers.AddQueryString(result, parametername, logins[i].ToLower());
            }
            return result;
        }
        private void GetAccessToken()
        {
            if (_accessTokenExpiration.HasValue && _accessTokenExpiration < DateTime.Now)
                return;

            var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "client_id",  _clientId ?? throw new Exception("ClientID not provided")},
                        { "client_secret", _clientSecret ?? throw new Exception("Client Secret not provided")},
                        { "grant_type", _grantType },

                    });

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://id.twitch.tv/"),
                Timeout =new TimeSpan(0,0,30)
            };

            var response = httpClient.PostAsync("oauth2/token", requestContent).GetAwaiter().GetResult();
            var accessToken = JsonConvert.DeserializeObject<TwitchAccessToken>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            _accessToken = accessToken;
            _accessTokenExpiration = DateTime.Now.AddSeconds(accessToken?.ExpiresIn ?? 0);
        }
        private HttpRequestMessage GetHttpRequestMessage(string path, HttpMethod method) =>
     new HttpRequestMessage { RequestUri = new Uri(_httpClient?.BaseAddress ?? throw new Exception("Http client not initialised"), path), Method = method }
     .AddAuthenticationHeader(AccessToken).AddClientIdHeader(_clientId);
    }
}
