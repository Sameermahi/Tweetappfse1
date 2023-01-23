using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweets.API.Controller;
using Tweets.API.Models;
using Tweets.API.Models.DTO;
using Tweets.API.Provider.IProvider;

namespace NUnitTestTweets.API
{
    public class TweetControllerTest
    {
        private Mock<ITweetProvider> _config;
        private TweetController _controller;
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
            _config = new Mock<ITweetProvider>();
            _controller = new TweetController(_config.Object);
        }

        [Test]
        public void GetAllTweets_WhenCalled_ReturnsListOfTweets()
        {
            _config.Setup(repo => repo.GetAllTweets()).Returns(Task.FromResult(Tweets));

            var result = _controller.GetAllTweets();
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [TestCase("Sameer")]
        [TestCase("Vamsi")]
        public void GetTweetsByUsername_GivenValidUserName_ReturnsOkObjectResult(string username)
        {
            _config.Setup(p => p.GetTweetsByUsername(username)).Returns(Task.FromResult(Tweets.Where(x => x.UserName == username)));

            var result = _controller.GetTweetsByUsername(username);

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        [TestCase("Abc")]
        [TestCase("DEF")]
        public void GetTweetsByUsername_GivenInValidUserName_ReturnsNotFoundResult(string username)
        {
            IEnumerable<Tweet> res = new List<Tweet> { };
            _config.Setup(p => p.GetTweetsByUsername(username)).Returns(Task.FromResult(res));
            var result = _controller.GetTweetsByUsername(username);

            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void AddTweet_ValidObject_ReturnsOkObjectResult()
        {
            TweetDTO tweet = new TweetDTO();
            _config.Setup(p => p.AddTweet(tweet)).Returns(Task.FromResult(true));
            var result = _controller.AddTweet(tweet);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void AddTweet_InValidObject_ReturnsConflictResult()
        {
            TweetDTO tweet = new TweetDTO();
            _config.Setup(p => p.AddTweet(tweet)).Returns(Task.FromResult(false));
            var result = _controller.AddTweet(tweet);
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void AddTweet_InValidObject_ReturnsBadResult()
        {
            var result = _controller.AddTweet(null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void UpdateTweet_ValidObject_ReturnsOkObjectResult()
        {
            TweetDTO tweet = new TweetDTO();
            Guid id = new Guid();
            _config.Setup(p => p.UpdateTweet(tweet,id)).Returns(Task.FromResult(true));
            var result = _controller.UpdateTweet(id,tweet);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void UpdateTweet_InValidObject_ReturnsConflictResult()
        {
            TweetDTO tweet = new TweetDTO();
            Guid id = new Guid();
            _config.Setup(p => p.UpdateTweet(tweet, id)).Returns(Task.FromResult(false));
            var result = _controller.UpdateTweet(id,tweet);
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void UpdateTweet_NullObject_ReturnsBadResult()
        {
            Guid id = new Guid();
            var result = _controller.UpdateTweet(id,null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void DeleteTweet_ValidId_ReturnsOkObjectResult()
        {
            Guid id = new Guid();
            _config.Setup(p => p.DeleteTweet(id)).Returns(Task.FromResult(true));
            var result = _controller.DeleteTweet(id);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void DeleteTweet_InValidId_ReturnsConflictResult()
        {
            Guid id = new Guid();
            _config.Setup(p => p.DeleteTweet(id)).Returns(Task.FromResult(false));
            var result = _controller.DeleteTweet(id);
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void LikeTweet_ValidId_ReturnsOkObjectResult()
        {
            Guid id = new Guid();
            _config.Setup(p => p.LikeTweet(id)).Returns(Task.FromResult(true));
            var result = _controller.LikeTweet(id);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void LikeTweet_InValidId_ReturnsConflictResult()
        {
            Guid id = new Guid();
            _config.Setup(p => p.LikeTweet(id)).Returns(Task.FromResult(false));
            var result = _controller.LikeTweet(id);
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void ReplyTweet_ValidObject_ReturnsOkObjectResult()
        {
            Reply reply = new Reply();
            Guid id = new Guid();
            _config.Setup(p => p.ReplyToTweet(reply, id)).Returns(Task.FromResult(true));
            var result = _controller.ReplyToTweet(id, reply);
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void ReplyTweet_InValidObject_ReturnsConflictResult()
        {
            Reply reply = new Reply();
            Guid id = new Guid();
            _config.Setup(p => p.ReplyToTweet(reply, id)).Returns(Task.FromResult(false));
            var result = _controller.ReplyToTweet(id, reply);
            Assert.That(result, Is.InstanceOf<StatusCodeResult>());
        }

        [Test]
        public void ReplyTweet_NullObject_ReturnsBadResult()
        {
            Guid id = new Guid();
            var result = _controller.UpdateTweet(id, null);
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}