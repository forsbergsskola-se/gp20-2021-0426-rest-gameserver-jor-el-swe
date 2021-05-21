using System;
using System.Text.Json.Serialization;

namespace GameRestAPI {
    class Organization {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("company")]
        public string Company { get; set; }

        [JsonPropertyName("blog")]
        public Uri Blog { get; set; }

        [JsonPropertyName("location")]
        public Uri Location { get; set; }

        [JsonPropertyName("email")]
        public int Email { get; set; }
        
    }
}