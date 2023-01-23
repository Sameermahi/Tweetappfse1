using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Tweets.API.Provider.IProvider;

namespace Tweets.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TweetController));
        public readonly ITweetProvider _tweetProvider;

        public TweetController(ITweetProvider tweetProvider) => _tweetProvider = tweetProvider;

        /// <summary>
        /// This method returns list of all tweets [Authorization Required]
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
        public IActionResult GetAllTweets()
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(GetAllTweets) + " method");
            try
            {
                var result = _tweetProvider.GetAllTweets().Result;
                if (result.Count() != 0)
                {
                    _log4net.Info("Tweets fetched Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(GetAllTweets) + " method");
                    return Ok(result);
                }
                else
                {
                    _log4net.Warn("No tweets found");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(GetAllTweets) + " method");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Tweets fetch failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(GetAllTweets) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method returns list of all tweets by username [Authorization Required]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTweetsByUsername(string UserName)
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(GetTweetsByUsername) + " method");
            try
            {
                var result = _tweetProvider.GetTweetsByUsername(UserName).Result;
                if (result.Count() != 0)
                {
                    _log4net.Info("Tweets by User name fetched Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(GetTweetsByUsername) + " method");
                    return Ok(result);
                }
                else
                {
                    _log4net.Warn("No tweets found by the username");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(GetTweetsByUsername) + " method");
                    return StatusCode(StatusCodes.Status204NoContent);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Tweets fetch by username failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(GetTweetsByUsername) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method adds a new tweet [Authorization Required]
        /// </summary>
        /// <param name="tweet"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddTweet([FromBody] TweetDTO tweet)
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(AddTweet) + " method");
            try
            {
                if (tweet == null)
                {
                    _log4net.Warn("Input tweet object is null");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(AddTweet) + " method");
                    return BadRequest("Invalid client request");
                }
                var result = _tweetProvider.AddTweet(tweet).Result;
                if (result == true)
                {
                    _log4net.Info("Added Tweet Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(AddTweet) + " method");
                    return Ok("Added Tweet Successfully");
                }
                else
                {
                    _log4net.Warn("Adding Tweet failed");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(AddTweet) + " method");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Tweet addition failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(AddTweet) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method updates a tweet [Authorization Required]
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="tweet"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTweet(Guid id,[FromBody] TweetDTO tweet)
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(UpdateTweet) + " method");
            try
            {
                if (tweet == null)
                {
                    _log4net.Warn("Input tweet object is null");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(UpdateTweet) + " method");
                    return BadRequest("Invalid client request");
                }
                var result = _tweetProvider.UpdateTweet(tweet,id).Result;
                if (result == true)
                {
                    _log4net.Info("Updated Tweet Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(UpdateTweet) + " method");
                    return Ok("Updated Tweet Successfully");
                }
                else
                {
                    _log4net.Warn("Updating Tweet failed");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(UpdateTweet) + " method");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Updating tweet failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(UpdateTweet) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
        /// <summary>
        /// This method deletes a tweet [Authorization Required]
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Abcd</returns>
        [HttpDelete]
        [Route("delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTweet(Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(DeleteTweet) + " method");
            try
            {
                var result = _tweetProvider.DeleteTweet(id).Result;
                if (result == true)
                {
                    _log4net.Info("Deleted Tweet Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(DeleteTweet) + " method");
                    return Ok("Deleted Tweet Successfully");
                }
                else
                {
                    _log4net.Warn("Deleting Tweet failed");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(DeleteTweet) + " method");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Deleting tweet failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(DeleteTweet) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method likes a tweet [Authorization Required]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("like")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult LikeTweet(Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(LikeTweet) + " method");
            try
            {
                var result = _tweetProvider.LikeTweet(id).Result;
                if (result == true)
                {
                    _log4net.Info("Liked Tweet Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(LikeTweet) + " method");
                    return Ok("Liked Tweet Successfully");
                }
                else
                {
                    _log4net.Warn("Liking Tweet failed");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(LikeTweet) + " method");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Liking tweet failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(LikeTweet) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// This method adds a reply to a tweet [Authorization Required]
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reply"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reply")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ReplyToTweet(Guid id,[FromBody] Reply reply)
        {
            _log4net.Info("Inside " + nameof(TweetController) + " - " + nameof(ReplyToTweet) + " method");
            try
            {
                if (reply == null)
                {
                    _log4net.Warn("Input reply object is null");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(ReplyToTweet) + " method");
                    return BadRequest("Invalid client request");
                }
                var result = _tweetProvider.ReplyToTweet(reply,id).Result;
                if (result == true)
                {
                    _log4net.Info("Replied Tweet Successfully");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(ReplyToTweet) + " method");
                    return Ok("Replied Tweet Successfully");
                }
                else
                {
                    _log4net.Warn("Replying Tweet failed");
                    _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(ReplyToTweet) + " method");
                    return StatusCode(StatusCodes.Status409Conflict);
                }
            }
            catch (Exception e)
            {
                _log4net.Error("Replying tweet failed with error: " + e.Message);
                _log4net.Info("End of " + nameof(TweetController) + " - " + nameof(ReplyToTweet) + " method");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
