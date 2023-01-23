using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Tweets.API.Provider.IProvider;
using Tweets.API.Repository.IRepository;

namespace Tweets.API.Provider
{
    public class TweetProvider:ITweetProvider
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserProvider));
        private readonly ITweetRepository _tweetRepository;

        public TweetProvider(ITweetRepository tweetRepository) => _tweetRepository = tweetRepository;

        public async Task<IEnumerable<Tweet>> GetAllTweets()
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(GetAllTweets) + " method");
            try
            {
                var result = await _tweetRepository.GetAllTweets();
                _log4net.Info("Successfully fetched all tweets");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(GetAllTweets) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Fetching all tweets Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(GetAllTweets) + " method");
                throw;
            }
        }
        public async Task<IEnumerable<Tweet>> GetTweetsByUsername(string UserName)
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(GetTweetsByUsername) + " method");
            try
            {
                var result = await _tweetRepository.GetTweetsByUsername(UserName);
                _log4net.Info("Successfully fetched all tweets by user");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(GetTweetsByUsername) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Fetching all tweets by user Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(GetTweetsByUsername) + " method");
                throw;
            }
        }
        public async Task<bool> AddTweet(TweetDTO tweet)
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(AddTweet) + " method");
            try
            {
                var result = await _tweetRepository.AddTweet(tweet);
                _log4net.Info("Added Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(AddTweet) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Adding tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(AddTweet) + " method");
                throw;
            }
        }
        public async Task<bool> UpdateTweet(TweetDTO tweet, Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(UpdateTweet) + " method");
            try
            {
                var result = await _tweetRepository.UpdateTweet(tweet,id);
                _log4net.Info("Updated Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(UpdateTweet) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Updating tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(UpdateTweet) + " method");
                throw;
            }
        }
        public async Task<bool> DeleteTweet(Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(DeleteTweet) + " method");
            try
            {
                var result = await _tweetRepository.DeleteTweet(id);
                _log4net.Info("Deleted Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(DeleteTweet) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Deleting tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(DeleteTweet) + " method");
                throw;
            }
        }

        public async Task<bool> LikeTweet(Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(LikeTweet) + " method");
            try
            {
                var result = await _tweetRepository.LikeTweet(id);
                _log4net.Info("Liked Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(LikeTweet) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Liking tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(LikeTweet) + " method");
                throw;
            }
        }

        public async Task<bool> ReplyToTweet(Reply reply,Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetProvider) + " - " + nameof(LikeTweet) + " method");
            try
            {
                var result = await _tweetRepository.ReplyToTweet(reply, id);
                _log4net.Info("Replied to Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(LikeTweet) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Replying to tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetProvider) + " - " + nameof(LikeTweet) + " method");
                throw;
            }
        }
    }
}
