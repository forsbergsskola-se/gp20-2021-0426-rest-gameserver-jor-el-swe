using System;
using System.Text.Json.Serialization;

namespace GameRestAPI {
    class Follower {

        [JsonPropertyName("login")]
        public string Login { get; set; }
        
        [JsonPropertyName("url")]
        public string URL { get; set; }

        [JsonPropertyName("followers_url")]
        public Uri FollowersURL { get; set; }
        
    }
}