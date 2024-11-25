using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlueskySharp.EndPoints
{
    public class Embed
    {
        [JsonPropertyName("$type")]
        public string Type
        {
            get;
            set;
        }

        public AttachedImage[] Images
        {
            get;
            set;
        }
    }
}
