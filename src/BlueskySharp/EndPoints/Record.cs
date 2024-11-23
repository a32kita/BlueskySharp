using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

using BlueskySharp.CustomCovertersAndPolicies;

namespace BlueskySharp.EndPoints
{
    public class Record
    {
        public string Text
        {
            get;
            set;
        }

        [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
        public DateTimeOffset CreatedAt
        {
            get;
            set;
        }
    }
}
