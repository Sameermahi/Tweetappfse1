using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Models;
using Tweets.API.Models.DTO;

namespace Tweets.API.Provider.IProvider
{
    public interface ITweetProvider
    {
        Task<IEnumerable<Tweet>> GetAllTweets();
        Task<IEnumerable<Tweet>> GetTweetsByUsername(string UserName);
        Task<bool> AddTweet(TweetDTO tweet);
        Task<bool> UpdateTweet(TweetDTO tweet, Guid id);
        Task<bool> DeleteTweet(Guid id);
        Task<bool> LikeTweet(Guid id);
        Task<bool> ReplyToTweet(Reply reply,Guid id);
    }
}
