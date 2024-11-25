using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlueskySharp.Endpoints
{
    public class Ref
    {
        [JsonPropertyName("$link")]
        public string Link
        {
            get;
            set;
        }
    }
}

