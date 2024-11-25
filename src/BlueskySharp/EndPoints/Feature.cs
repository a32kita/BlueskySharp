using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlueskySharp.EndPoints
{
    public class Feature
    {
        [JsonPropertyName("$type")]
        public string Type
        {
            get;
            set;
        }

        public Uri Uri
        {
            get;
            set;
        }
    }
}
