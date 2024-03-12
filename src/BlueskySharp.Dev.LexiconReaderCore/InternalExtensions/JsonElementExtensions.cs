using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace BlueskySharp.Dev.LexiconReaderCore.InternalExtensions
{
    internal static class JsonElementExtensions
    {
        public static string GetPropertyStringOrDefault(this JsonElement jsonElement, string propertyName, string defaultValue = null)
        {
            JsonElement outputElement;
            if (jsonElement.TryGetProperty(propertyName, out outputElement))
                return outputElement.GetString();

            return defaultValue;
        }

        public static JsonElement? GetPropertyElementOrNull(this JsonElement jsonElement, string propertyName)
        {
            JsonElement outputElement;
            if (jsonElement.TryGetProperty(propertyName, out outputElement))
                return outputElement;

            return null;
        }
    }
}
