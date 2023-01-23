using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Tweets.API.Repository.IRepository;

namespace Tweets.API.Repository
{
    public class TweetRepository : ITweetRepository
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(UserRepository));
        private readonly IMongoCollection<Tweet> _tweetCollection;
        public TweetRepository(IOptions<TweetDatabaseSetting> tweetDatabaseSettings)
        {
            var client = new MongoClient(tweetDatabaseSettings.Value.ConnectionString);

            var database = client.GetDatabase(tweetDatabaseSettings.Value.DatabaseName);

            _tweetCollection = database.GetCollection<Tweet>(tweetDatabaseSettings.Value.TweetsCollectionName);

        }
        public async Task<IEnumerable<Tweet>> GetAllTweets()
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(GetAllTweets) + " method");
            try
            {
                var result = await _tweetCollection.Find(_ => true).ToListAsync();
                _log4net.Info("Successfully fetched all tweets");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(GetAllTweets) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Fetching all tweets Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(GetAllTweets) + " method");
                throw;
            }
        }
        public async Task<IEnumerable<Tweet>> GetTweetsByUsername(string UserName)
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(GetTweetsByUsername) + " method");
            try
            {
                var result = await _tweetCollection.Find<Tweet>(tweet => (tweet.UserName == UserName)).ToListAsync();
                _log4net.Info("Fetched tweets by username Successfully");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(GetTweetsByUsername) + " method");
                return result;
            }
            catch (Exception e)
            {
                _log4net.Error("Fetching tweets by username Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(GetTweetsByUsername) + " method");
                throw;
            }
        }
        public async Task<bool> AddTweet(TweetDTO tweet)
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(AddTweet) + " method");
            try
            {
                Tweet newTweet = new Tweet();
                newTweet.Id = new Guid();
                newTweet.UserName = tweet.UserName;
                newTweet.Message = tweet.Message;
                newTweet.Date = DateTime.Now;
                newTweet.Like = 0;
                newTweet.Reply = new List<Reply> { };
                await _tweetCollection.InsertOneAsync(newTweet);
                _log4net.Info("Added Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(AddTweet) + " method");
                return true;
            }
            catch (Exception e)
            {
                _log4net.Error("Adding tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(AddTweet) + " method");
                throw;
            }
        }
        public async Task<bool> UpdateTweet(TweetDTO tweet, Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(UpdateTweet) + " method");
            try
            {
                Tweet tweetFromDb = await _tweetCollection.Find<Tweet>(tweet => (tweet.Id == id)).FirstOrDefaultAsync();
                tweetFromDb.Message = tweet.Message;
                tweetFromDb.Date = DateTime.Now;
                await _tweetCollection.ReplaceOneAsync(x => x.Id == id, tweetFromDb);
                _log4net.Info("Updated Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(UpdateTweet) + " method");
                return true;
            }
            catch (Exception e)
            {
                _log4net.Error("Updating tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(UpdateTweet) + " method");
                throw;
            }
        }
        public async Task<bool> DeleteTweet(Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(DeleteTweet) + " method");
            try
            {
                var result = await _tweetCollection.DeleteOneAsync(tweet => (tweet.Id == id));
                _log4net.Info("Deleted tweet Successfully");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(DeleteTweet) + " method");
                return true;
            }
            catch (Exception e)
            {
                _log4net.Error("Deleting tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(DeleteTweet) + " method");
                throw;
            }
        }
        
        public async Task<bool> LikeTweet(Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(LikeTweet) + " method");
            try
            {
                Tweet tweetFromDb = await _tweetCollection.Find<Tweet>(tweet => (tweet.Id == id)).FirstOrDefaultAsync();
                tweetFromDb.Like += 1;
                await _tweetCollection.ReplaceOneAsync(x => x.Id == id, tweetFromDb);
                _log4net.Info("Liked Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(LikeTweet) + " method");
                return true;
            }
            catch (Exception e)
            {
                _log4net.Error("Liking tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(LikeTweet) + " method");
                throw;
            }
        }

        public async Task<bool> ReplyToTweet(Reply reply,Guid id)
        {
            _log4net.Info("Inside " + nameof(TweetRepository) + " - " + nameof(ReplyToTweet) + " method");
            try
            {
                Tweet tweetFromDb = await _tweetCollection.Find<Tweet>(tweet => (tweet.Id == id)).FirstOrDefaultAsync();
                tweetFromDb.Reply.Add(reply);
                await _tweetCollection.ReplaceOneAsync(x => x.Id == id, tweetFromDb);
                _log4net.Info("Replied to Tweet Successfully");
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(ReplyToTweet) + " method");
                return true;
            }
            catch (Exception e)
            {
                _log4net.Error("Replying to tweet Failed with Error : " + e.Message);
                _log4net.Info("End of " + nameof(TweetRepository) + " - " + nameof(ReplyToTweet) + " method");
                throw;
            }
        }
    }
}
