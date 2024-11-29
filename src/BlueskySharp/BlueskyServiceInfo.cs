using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp
{
    public class BlueskyServiceInfo
    {
        private string _baseUrl;

        public static readonly BlueskyServiceInfo BskySocial = new BlueskyServiceInfo() { BaseUrl = "https://bsky.social" };

        public string BaseUrl
        {
            get => this._baseUrl;
            set => this._baseUrl = value.TrimEnd('/');
        }

        public Uri GetFullUri(string path)
        {
            return new Uri(this._baseUrl + '/' + path.TrimStart('/'));
        }
    }
}
