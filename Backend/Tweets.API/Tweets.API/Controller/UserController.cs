using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Provider.IProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Transactions;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Tweets.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserController));
        public readonly IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        /// <summary>
        /// This method registers a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody]UserDTO user)
        {
            _log4net.Info("Inside " + nameof(UserController) + " - " + nameof(Register) + " method");
            try
            {
                if (user == null)
                {
                    _log4net.Warn("Input User object is null");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Register) + " method");
                    return BadRequest("Invalid client request");
                }
                var result = _userProvider.Register(user).Result;
                if(result == true)
                {
                    _log4net.Info("User Successfully Registered");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Register) + " method");
                    return Ok("User Registration Successfull");
                }
                else
                {
                    _log4net.Warn("User Registration failed");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Register) + " method");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User Registration failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Register) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method login's a user
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login(string UserName,string password)
        {
            _log4net.Info("Inside " + nameof(UserController) + " - " + nameof(Login) + " method");
            try
            {
                if (UserName == "" || password == "")
                {
                    _log4net.Warn("UserName or Password is empty");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Login) + " method");
                    return BadRequest("Invalid client request");
                }
                var result = _userProvider.Login(UserName,password).Result;
                if (result == true)
                {
                    _log4net.Info("User logged in Successfully");
                    string token = _userProvider.GetToken();
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Login) + " method");
                    return Ok(new { Response = "User Login Successfull" ,Token = token});
                }
                else
                {
                    _log4net.Warn("User Login failed");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(Login) + " method");
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User Login failed with error: " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method updates the password of a user for the given Username
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ForgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ForgotPassword(string UserName, string newPassword)
        {
            _log4net.Info("Inside " + nameof(UserController) + " - " + nameof(ForgotPassword) + " method");
            try
            {
                if (UserName == "" || newPassword == "")
                {
                    _log4net.Warn("UserName or Password is empty");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(ForgotPassword) + " method");
                    return BadRequest("Invalid client request");
                }
                var result = _userProvider.ForgotPassword(UserName, newPassword).Result;
                if (result == true)
                {
                    _log4net.Info("User password changed Successfully");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(ForgotPassword) + " method");
                    return Ok("Password change Successfull");
                }
                else
                {
                    _log4net.Warn("User Password change failed");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(ForgotPassword) + " method");
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("User Password change failed with error: " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// This method returns list of all users [Authorization Required]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers()
        {
            _log4net.Info("Inside " + nameof(UserController) + " - " + nameof(GetAllUsers) + " method");
            try
            {
                var result = _userProvider.GetAllUsers().Result;
                if (result.Count() != 0)
                {
                    _log4net.Info("Users fetched Successfully");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(GetAllUsers) + " method");
                    return Ok(result);
                }
                else
                {
                    _log4net.Warn("No users found");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(GetAllUsers) + " method");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Users fetch failed with error: " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method searches for users by user name [Authorization Required]
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchUser(string UserName)
        {
            _log4net.Info("Inside " + nameof(UserController) + " - " + nameof(SearchUser) + " method");
            try
            {
                var result = _userProvider.SearchUser(UserName).Result;
                if (result.Count() != 0)
                {
                    _log4net.Info("Users by username fetched Successfully");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(SearchUser) + " method");
                    return Ok(result);
                }
                else
                {
                    _log4net.Warn("No users found");
                    _log4net.Info("End of " + nameof(UserController) + " - " + nameof(SearchUser) + " method");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Users search failed with error: " + e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
