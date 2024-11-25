using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

using BlueskySharp.CustomCovertersAndPolicies;
using BlueskySharp.EndPoints.InternalHelpers;

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

        public Embed Embed
        {
            get;
            set;
        }

        public Facet[] Facets
        {
            get;
            set;
        }


        public static Record FromMarkdownText(string text)
        {
            var result = MarkdownParseResult.ParseMarkdownToFacets(text);

            return new Record()
            {
                CreatedAt = DateTimeOffset.Now,
                Facets = result.Facets,
                Text = result.ParsedText,
            };
        }
    }
}
