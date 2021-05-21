using System;
using System.Text.Json.Serialization;

namespace GameRestAPI {
    class Organization {
        [JsonPropertyName("login")]
        public string Login { get; set; }
        
        [JsonPropertyName("url")]
        public string URL { get; set; }

        [JsonPropertyName("description")]
        public Uri Description { get; set; }
        
    }
}