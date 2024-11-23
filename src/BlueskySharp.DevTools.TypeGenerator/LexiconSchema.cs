using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueskySharp.DevTools.TypeGenerator
{
    public class LexiconSchema
    {
        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, JsonElement> Properties { get; set; }

        [JsonPropertyName("required")]
        public List<string> Required { get; set; }
    }
}
