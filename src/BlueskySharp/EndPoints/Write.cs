using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlueskySharp.EndPoints
{
    public class Write
    {
        [JsonPropertyName("$type")]
        public string Type
        {
            get;
            set;
        }

        public string Collection
        {
            get;
            set;
        }

        public string Rkey
        {
            get;
            set;
        }

        public Record Value
        {
            get;
            set;
        }
    }
}
