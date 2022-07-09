using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TwitchIntegration.Model;

namespace TwitchIntegration
{
    internal static class HttpClientExtensions
    {
        public static HttpRequestMessage AddStringContent(this HttpRequestMessage message, string content)
        {
            message.Content = new StringContent(content);
            return message;
        }
        public static HttpRequestMessage AddStringContent(this HttpRequestMessage message, object content, string mediaType)
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, mediaType);
            return message;
        }
        public static HttpRequestMessage AddAuthenticationHeader(this HttpRequestMessage message, TwitchAccessToken token)
        {
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
            return message;
        }
        public static HttpRequestMessage AddClientIdHeader(this HttpRequestMessage message, string clientId)
        {
            message.Headers.Add("Client-Id", clientId);
            return message;
        }
        public static async Task<T?> ReadAsAsync<T>(this HttpContent httpContent)
        {
            var stringContent = await httpContent.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(stringContent ?? throw new Exception("No string content to read")) ?? default;
        }
    }
}
