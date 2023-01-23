using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweets.API.Models
{
    public class Reply
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
