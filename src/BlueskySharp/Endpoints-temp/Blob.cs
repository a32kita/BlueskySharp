using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlueskySharp.Endpoints
{
    public class Blob
    {
        [JsonPropertyName("$type")]
        public string Type
        {
            get;
            set;
        } = "blob";

        public string MimeType
        {
            get;
            set;
        }

        public Ref Ref
        {
            get;
            set;
        }

        public long Size
        {
            get;
            set;
        }
    }
}

