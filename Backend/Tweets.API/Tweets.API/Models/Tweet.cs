using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Tweets.API.Models
{
    public class Tweet
    {
        [BsonId]
        public Guid Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int Like { get; set; }

        public List<Reply> Reply { get; set; }
    }
}
