using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweets.API.Models
{
    public interface ITweetDatabaseSetting
    {
        string ConnectionString { get; set; }

        string DatabaseName { get; set; }

        string TweetsCollectionName { get; set; }

        string UsersCollectionName { get; set; }

    }
}
