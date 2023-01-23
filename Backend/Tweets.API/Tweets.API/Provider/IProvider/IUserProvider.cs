using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using Tweets.API.Models.DTO;

namespace Tweets.API.Provider.IProvider
{
    public interface IUserProvider
    {
        Task<bool> Register(UserDTO user);
        Task<bool> Login(string UserName, string password);
        string GetToken();
        Task<bool> ForgotPassword(string UserName, string newPassword);
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> SearchUser(string UserName);
    }
}
