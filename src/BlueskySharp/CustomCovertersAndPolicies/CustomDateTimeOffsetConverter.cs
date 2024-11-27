using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlueskySharp.CustomCovertersAndPolicies
{
    public class CustomDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();

            try
            {
                return DateTimeOffset.ParseExact(stringValue.Replace("T", " "), "u", null);
            }
            catch
            {
                var format = "yyyy-MM-dd HH:mm:ss.fffZ";
                return DateTimeOffset.ParseExact(stringValue.Replace("T", " "), format, null);
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("u").Replace(" ", "T"));
        }
    }
}
