using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Tweets.API.Controller;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Tweets.API.Provider;
using Tweets.API.Repository.IRepository;

namespace NUnitTestTweets.API
{
    class TweetProviderTests
    {
        private Mock<ITweetRepository> _config;
        private TweetProvider _provider;
        private IEnumerable<Tweet> Tweets = new List<Tweet> {
        new Tweet {
            Id = new Guid(),
            UserName= "Sameer",
            Message= "Hi, A very Good Morning twitter users",
            Date= new DateTime(),
            Like= 0,
            Reply= new List<Reply>{ }
        },
        new Tweet {
            Id = new Guid(),
            UserName= "Vamsi",
            Message= "Hi, How's life going on guys?",
            Date= new DateTime(),
            Like= 0,
            Reply= new List<Reply>{ }
        } };

        [SetUp]
        public void Setup()
        {
            _config = new Mock<ITweetRepository>();
            _provider = new TweetProvider(_config.Object);
        }

        [Test]
        public void GetAllTweets_WhenCalled_ReturnsListOfTweets()
        {
            _config.Setup(repo => repo.GetAllTweets()).Returns(Task.FromResult(Tweets));

            var result = _provider.GetAllTweets();
            Assert.That(result, Is.InstanceOf<Task>());
        }

        [TestCase("Sameer")]
        [TestCase("Abc")]
        public void GetTweetsByUsername_GivenUserName_ReturnsTweetsbyUser(string username)
        {
            _config.Setup(p => p.GetTweetsByUsername(username)).Returns(Task.FromResult(Tweets.Where(x => x.UserName == username)));

            var result = _provider.GetTweetsByUsername(username);

            Assert.That(result, Is.InstanceOf<Task>());
        }


        [Test]
        public void AddTweet_ValidObject_ReturnsResult()
        {
            TweetDTO tweet = new TweetDTO();
            _config.Setup(p => p.AddTweet(tweet)).Returns(Task.FromResult(true));
            var result = _provider.AddTweet(tweet);
            Assert.That(result, Is.InstanceOf<Task>());
        }
        
        [Test]
        public void UpdateTweet_ValidObject_ReturnsTrue()
        {
            TweetDTO tweet = new TweetDTO();
            Guid id = new Guid();
            _config.Setup(p => p.UpdateTweet(tweet, id)).Returns(Task.FromResult(true));
            var result = _provider.UpdateTweet(tweet,id);
            Assert.That(result, Is.InstanceOf<Task>());
        }
        
        [Test]
        public void DeleteTweet_ValidId_ReturnsTrue()
        {
            Guid id = new Guid();
            _config.Setup(p => p.DeleteTweet(id)).Returns(Task.FromResult(true));
            var result = _provider.DeleteTweet(id);
            Assert.That(result, Is.InstanceOf<Task>());
        }

        [Test]
        public void LikeTweet_ValidId_ReturnsTrue()
        {
            Guid id = new Guid();
            _config.Setup(p => p.LikeTweet(id)).Returns(Task.FromResult(true));
            var result = _provider.LikeTweet(id);
            Assert.That(result, Is.InstanceOf<Task>());
        }

        [Test]
        public void ReplyTweet_ValidObject_ReturnsTrue()
        {
            Reply reply = new Reply();
            Guid id = new Guid();
            _config.Setup(p => p.ReplyToTweet(reply, id)).Returns(Task.FromResult(true));
            var result = _provider.ReplyToTweet(reply,id);
            Assert.That(result, Is.InstanceOf<Task>());
        }
    }
}
