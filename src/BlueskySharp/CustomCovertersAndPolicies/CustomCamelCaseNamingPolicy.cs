using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BlueskySharp.CustomCovertersAndPolicies
{
    public class CustomCamelCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
                return name;

            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}
