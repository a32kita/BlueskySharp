using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using BlueskySharp.CustomCovertersAndPolicies;

namespace BlueskySharp.EndPoints
{
    public abstract class EndPointBase
    {
        internal static readonly JsonSerializerOptions DefaultJsonSerializerOption
            = new JsonSerializerOptions() {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new CustomCamelCaseNamingPolicy(),
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
#if DEBUG
                WriteIndented = true,
#endif
            };

        internal static readonly MediaTypeHeaderValue DefaultMediaTypeHeaderValue = new MediaTypeHeaderValue("application/json");

        internal static readonly Encoding UTF8WithOutBOMEncoding = new UTF8Encoding(false);


        private BlueskyService _parent;

        protected BlueskyServiceInfo ServiceInfo
        {
            get => this._parent.ServiceInfo;
        }

        protected HttpClient HttpClient
        {
            get => this._parent.HttpClient;
        }

        protected BlueskySessionInfo SessionInfo
        {
            get => this._parent.SessionInfo;
        }


        protected EndPointBase(BlueskyService parent)
        {
            this._parent = parent;
        }


        protected async Task<TResult> InvokeProcedureAsync<TParam, TResult>(string path, TParam param, bool onSession = true)
        {
            var fullUri = this.ServiceInfo.GetFullUri(path);

            using (var ms = new MemoryStream())
            {
#if DEBUG
                var jsonString = JsonSerializer.Serialize(param, DefaultJsonSerializerOption);

                using (var sw = new StreamWriter(ms, UTF8WithOutBOMEncoding, 512, true))
                {
                    sw.Write(jsonString);
                    sw.Flush();
                }
#else
                JsonSerializer.Serialize(ms, param, DefaultJsonSerializerOption);
#endif
                ms.Seek(0, SeekOrigin.Begin);
                using (var streamContent = new StreamContent(ms))
                {
                    streamContent.Headers.ContentType = DefaultMediaTypeHeaderValue;

                    var hRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                    {
                        Content = streamContent,
                    };

                    if (onSession)
                    {
                        var token = this.SessionInfo?.AccessJwt;
                        if (String.IsNullOrEmpty(token))
                            throw new Exception("AccessJwt is empty.");

                        hRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    using (var hResponse = await this.HttpClient.SendAsync(hRequest))
                    {
#if DEBUG && true
                        var responseJson = await hResponse.Content.ReadAsStringAsync();

                        TResult deserializedResult = default(TResult);
                        try
                        {
                            deserializedResult = JsonSerializer.Deserialize<TResult>(responseJson, DefaultJsonSerializerOption);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }


                        if ((int)hResponse.StatusCode / 100 != 2)
                            hResponse.EnsureSuccessStatusCode();

                        return deserializedResult;
#else
                        hResponse.EnsureSuccessStatusCode();

                        using (var hResponseContentStream = await hResponse.Content.ReadAsStreamAsync())
                        {
                            var deserializedResult = JsonSerializer.Deserialize<TResult>(hResponseContentStream, DefaultJsonSerializerOption);
                            return deserializedResult;
                        }
#endif
                    }
                }
            }
        }
    }
}
