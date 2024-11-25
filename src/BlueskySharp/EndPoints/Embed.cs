using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

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

        public ExternalReference External
        {
            get;
            set;
        }


        public static Embed FromImages(IEnumerable<AttachedImage> images)
        {
            return new Embed()
            {
                Type = "app.bsky.embed.images",
                Images = images.ToArray()
            };
        }

        public static Embed FromExternal(ExternalReference external)
        {
            return new Embed()
            {
                Type = "app.bsky.embed.external",
                External = external
            };
        }
    }
}
