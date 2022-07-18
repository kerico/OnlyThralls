using Microsoft.AspNetCore.Components.Authorization;
using OnlyThrals.Authentication;
using OnlyThrals.Model;

namespace OnlyThrals.Data
{
    public interface ILoginService
    {
        Task<Login> LoginAsync(string username, string password);
        public Login User { get; set; }
    }
    public class LoginService : ILoginService
    {



        private static readonly List<Login> logins = new List<Login>
        {
            new Login{ Username ="kerico", Password = "sapphire"}
        };
        public Login User { get; set; }
        public async Task<Login> LoginAsync(string username, string password)
        {
            Login first = await Task.FromResult(logins.FirstOrDefault(user => user.Username.Equals(username) && password.Equals(password)));
            if (first == null)
            {
                throw new Exception("User not found");
            }

            return first;
        }
    }
}
