using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweets.API.Models
{
    public class TweetDatabaseSetting : ITweetDatabaseSetting
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string TweetsCollectionName { get; set; }

        public string UsersCollectionName { get; set; }
    }
}
