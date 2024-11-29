using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlueskySharp.Endpoints.InternalHelpers
{
    internal static class UrlParameterConverter
    {
        public static string ToUrlParameters(object obj, string prefix = null)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var parameters = new List<string>();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value == null) continue;

                var propertyName = string.IsNullOrEmpty(prefix) ? property.Name.ToCamelCase() : $"{prefix}.{property.Name.ToCamelCase()}";

                if (value.GetType().IsClass && !value.GetType().Assembly.FullName.StartsWith("System"))
                {
                    parameters.Add(ToUrlParameters(value, propertyName));
                }
                else
                {
                    if (value is bool)
                        value = value.ToString().ToLower();

                    parameters.Add($"{propertyName}={Uri.EscapeDataString(value.ToString())}");
                }
            }

            return string.Join("&", parameters);
        }

        private static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
                return str;

            string camelCase = char.ToLower(str[0]) + str.Substring(1);
            return camelCase;
        }
    }
}
