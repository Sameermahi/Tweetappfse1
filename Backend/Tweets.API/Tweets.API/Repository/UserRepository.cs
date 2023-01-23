using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using MongoDB.Bson.Serialization;
using Tweets.API.Models.DTO;

namespace Tweets.API.Repository
{
    public class UserRepository : IUserRepository
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserRepository));
        private readonly IMongoCollection<User> _userCollection;
        public UserRepository(IOptions<TweetDatabaseSetting> tweetDatabaseSettings)
        {
            var client = new MongoClient(tweetDatabaseSettings.Value.ConnectionString);

            var database = client.GetDatabase(tweetDatabaseSettings.Value.DatabaseName);

            _userCollection = database.GetCollection<User>(tweetDatabaseSettings.Value.UsersCollectionName);

        }

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public async Task<bool> Register(UserDTO user)
        {
            _log4net.Info("Inside " + nameof(UserRepository) + " - " + nameof(Register) + " method");
            try
            {
                User newUser = new User();
                newUser.Id = new Guid();
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Email = user.Email;
                newUser.UserName = user.UserName;
                newUser.Password = EncodePasswordToBase64(user.Password);
                newUser.ContactNumber = user.ContactNumber;
                await _userCollection.InsertOneAsync(newUser);
                _log4net.Info("User Successfully Registered");
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(Register) + " method");
                return true;
            }
            catch(Exception e)
            {
                _log4net.Error("User Registration Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(Register) + " method");
                throw;
            }
        }

        public async Task<bool> Login(string UserName, string password)
        {
            _log4net.Info("Inside " + nameof(UserRepository) + " - " + nameof(Login) + " method");
            try
            {
                var result = await _userCollection.Find(user => (user.UserName == UserName && user.Password == EncodePasswordToBase64(password))).FirstOrDefaultAsync();
                if(result == null)
                {
                    _log4net.Warn("User Log in failed as credentials were wrong/not found");
                    _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(Login) + " method");
                    return false;
                }
                else
                {
                    _log4net.Info("User Successfully Logged in");
                    _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(Login) + " method");
                    return true;
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User Log in Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(Login) + " method");
                throw;
            }
        }

        public async Task<bool> ForgotPassword(string UserName, string newPassword)
        {
            _log4net.Info("Inside " + nameof(UserRepository) + " - " + nameof(ForgotPassword) + " method");
            try
            {
                var result = await _userCollection.Find<User>(user => (user.UserName == UserName)).FirstOrDefaultAsync();
                if (result == null)
                {
                    _log4net.Warn("User not able to change password as User with " + UserName + " not found.");
                    _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(ForgotPassword) + " method");
                    return false;
                }
                else
                {
                    result.Password = EncodePasswordToBase64(newPassword);
                    await _userCollection.ReplaceOneAsync(user => user.UserName == result.UserName,result);
                    _log4net.Info("User password changed Successfully");
                    _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(ForgotPassword) + " method");
                    return true;
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User password change Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(ForgotPassword) + " method");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            _log4net.Info("Inside " + nameof(UserRepository) + " - " + nameof(GetAllUsers) + " method");
            try
            {
                var result = await _userCollection.Find(_ => true).ToListAsync();
                _log4net.Info("Successfully fetched all users");
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(GetAllUsers) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Fetching all users Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(GetAllUsers) + " method");
                throw;
            }
        }

        public async Task<IEnumerable<User>> SearchUser(string UserName)
        {
            _log4net.Info("Inside " + nameof(UserRepository) + " - " + nameof(SearchUser) + " method");
            try
            {
                var queryExpr = new BsonRegularExpression(new Regex(UserName, RegexOptions.IgnoreCase));
                var filter = Builders<User>.Filter.Regex("UserName", queryExpr);
                var result = await _userCollection.FindSync(filter).ToListAsync();
                _log4net.Info("Successfully fetched users with given search input");
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(SearchUser) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Searching users Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(UserRepository) + " - " + nameof(SearchUser) + " method");
                throw;
            }
        }
    }
}
