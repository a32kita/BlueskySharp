using BlueskySharp.CustomCovertersAndPolicies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BlueskySharp.Endpoints
{
    public class ProfileView : BskyActor.BskyActorEndpoint.ProfileViewBasic
    {
        [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
        public DateTimeOffset IndexedAt
        {
            get;
            set;
        }
    }
}
