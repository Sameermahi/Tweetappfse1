using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Tweets.API.Provider.IProvider;
using Tweets.API.Repository;

namespace Tweets.API.Provider
{
    public class UserProvider : IUserProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserProvider));
        private readonly IUserRepository _userRepository;

        public UserProvider(IUserRepository userRepository) => _userRepository = userRepository;
        public async Task<bool> Register(UserDTO user)
        {
            _log4net.Info("Inside " + nameof(UserProvider) + " - " + nameof(Register) + " method");
            try
            {
                var result = await _userRepository.Register(user);
                if (result == true)
                {
                    _log4net.Info("User Successfully Registered");
                    _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(Register) + " method");
                    return true;
                }
                else
                {
                    _log4net.Warn("User Registration Failed");
                    _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(Register) + " method");
                    return false;
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User Registration Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(Register) + " method");
                throw;
            }
        }

        public async Task<bool> Login(string UserName, string password)
        {
            _log4net.Info("Inside " + nameof(UserProvider) + " - " + nameof(Login) + " method");
            try
            {
                var result = await _userRepository.Login(UserName,password);
                if (result == true)
                {
                    _log4net.Info("User Successfully Logged in");
                    _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(Login) + " method");
                    return true;
                }
                else
                {
                    _log4net.Warn("User Login Failed");
                    _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(Login) + " method");
                    return false;
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User Login Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(Login) + " method");
                throw;
            }
        }

        public string GetToken()
        {
            _log4net.Info("Inside " + nameof(UserProvider) + " - " + nameof(GetToken) + " method");
            try
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(GetToken) + " method");
                return token;
            }
            catch (Exception e)
            {
                _log4net.Error("Token Generation Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(GetToken) + " method");
                throw;
            }
        }

        public async Task<bool> ForgotPassword(string UserName, string newPassword)
        {
            _log4net.Info("Inside " + nameof(UserProvider) + " - " + nameof(ForgotPassword) + " method");
            try
            {
                var result = await _userRepository.ForgotPassword(UserName, newPassword);
                if (result == true)
                {
                    _log4net.Info("User password changed Successfully");
                    _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(ForgotPassword) + " method");
                    return true;
                }
                else
                {
                    _log4net.Warn("User password change failed");
                    _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(ForgotPassword) + " method");
                    return false;
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User password change Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(ForgotPassword) + " method");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            _log4net.Info("Inside " + nameof(UserProvider) + " - " + nameof(GetAllUsers) + " method");
            try
            {
                var result = await _userRepository.GetAllUsers();
                _log4net.Info("Successfully fetched all users");
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(GetAllUsers) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Fetching all users Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(GetAllUsers) + " method");
                throw;
            }
        }

        public async Task<IEnumerable<User>> SearchUser(string UserName)
        {
            _log4net.Info("Inside " + nameof(UserProvider) + " - " + nameof(SearchUser) + " method");
            try
            {
                var result = await _userRepository.SearchUser(UserName);
                _log4net.Info("Successfully fetched users with given search input");
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(SearchUser) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Searching users Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserProvider) + " - " + nameof(SearchUser) + " method");
                throw;
            }
        }
    }
}
