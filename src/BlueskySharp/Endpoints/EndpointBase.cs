﻿using System;
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

namespace BlueskySharp.Endpoints
{
    /// <summary>
    /// The base class for service endpoints. This class is used internally within the library.
    /// </summary>
    public abstract class EndpointBase
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

        /// <summary>
        /// An event that is triggered when a request to the endpoint occurs.
        /// </summary>
        public event EventHandler Calling;


        private BlueskyService _parent;

        /// <summary>
        /// Gets information about the target service.
        /// </summary>
        protected BlueskyServiceInfo ServiceInfo
        {
            get => this._parent.ServiceInfo;
        }

        /// <summary>
        /// Gets the HttpClient used for requests.
        /// </summary>
        protected HttpClient HttpClient
        {
            get => this._parent.HttpClient;
        }

        /// <summary>
        /// Gets session information.
        /// </summary>
        protected BlueskySessionInfo SessionInfo
        {
            get => this._parent.SessionInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointBase"/> class.
        /// </summary>
        /// <param name="parent">親となる <see cref="BlueskyService"/></param>
        protected EndpointBase(BlueskyService parent)
        {
            this._parent = parent;
        }

        /// <summary>
        /// Uploads content.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="mimeType">The MIME type of the content.</param>
        /// <param name="contentStream">The content stream.</param>
        /// <param name="disableCallingEvent">Specifies whether the triggering of the Calling event is disabled.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected async Task<TResult> UploadContentAsync<TResult>(string path, string mimeType, Stream contentStream, bool disableCallingEvent = false)
        {
            if (!disableCallingEvent)
                this.Calling?.Invoke(this, EventArgs.Empty);

            var fullUri = this.ServiceInfo.GetFullUri(path);

            using (var content = new StreamContent(contentStream))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                using (var hRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                {
                    Content = content
                })
                {
                    var token = this.SessionInfo?.AccessJwt;
                    if (String.IsNullOrEmpty(token))
                        throw new Exception("AccessJwt is empty.");

                    hRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var hResponse = await this.HttpClient.SendAsync(hRequest))
                    {
                        hResponse.EnsureSuccessStatusCode();

                        using (var hResponseContentStream = await hResponse.Content.ReadAsStreamAsync())
                        {
                            var deserializedResult = JsonSerializer.Deserialize<TResult>(hResponseContentStream, DefaultJsonSerializerOption);
                            return deserializedResult;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes a remote procedure.
        /// </summary>
        /// <typeparam name="TParam"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="path">The API path.</param>
        /// <param name="param">Parameters for the procedure.</param>
        /// <param name="onSession">Specifies whether to send session information.</param>
        /// <param name="bearer">Specify if the Bearer token to use differs from the default.</param>
        /// <param name="disableCallingEvent">Specifies whether the triggering of the Calling event is disabled.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        protected async Task<TResult> InvokeProcedureAsync<TParam, TResult>(string path, TParam param = default(TParam), bool onSession = true, string bearer = null, bool disableCallingEvent = false)
        {
            if (onSession && !String.IsNullOrEmpty(bearer))
                throw new ArgumentException();

            if (!disableCallingEvent)
                this.Calling?.Invoke(this, EventArgs.Empty);

            var fullUri = this.ServiceInfo.GetFullUri(path);

            using (var ms = new MemoryStream())
            {
                if (param is EmptyParam == false)
                {
#if DEBUG && false
                    var jsonString = JsonSerializer.Serialize(param, DefaultJsonSerializerOption);

                    using (var sw = new StreamWriter(ms, UTF8WithOutBOMEncoding, 512, true))
                    {
                        sw.Write(jsonString);
                        sw.Flush();
                    }
#else
                    JsonSerializer.Serialize(ms, param, DefaultJsonSerializerOption);
#endif
                }

                ms.Seek(0, SeekOrigin.Begin);
                using (var streamContent = new StreamContent(ms))
                {
                    streamContent.Headers.ContentType = DefaultMediaTypeHeaderValue;

                    using (var hRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                    {
                        Content = streamContent,
                    })
                    {
                        if (onSession)
                        {
                            var token = this.SessionInfo?.AccessJwt;
                            if (String.IsNullOrEmpty(token))
                                throw new Exception("AccessJwt is empty.");

                            hRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        }
                        else if (String.IsNullOrEmpty(bearer) == false)
                        {
                            hRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
                        }

                        using (var hResponse = await this.HttpClient.SendAsync(hRequest))
                        {
#if DEBUG && false
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
}


