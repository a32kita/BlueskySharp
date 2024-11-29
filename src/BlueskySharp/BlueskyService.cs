using BlueskySharp.Endpoints.Repo;
using BlueskySharp.Endpoints.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlueskySharp
{
    /// <summary>
    /// 
    /// </summary>
    public class BlueskyService : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Retrieves the type of authentication.
        /// </summary>
        public BlueskyAuthType AuthType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the JWT is automatically refreshed.
        /// </summary>
        public bool JwtAutoRefresh
        {
            get;
            set;
        }

        /// <summary>
        /// The "server" endpoint of the API.
        /// </summary>
        public ServerEndpoint Server
        {
            get;
            private set;
        }

        /// <summary>
        /// The "repo" endpoint of the API.
        /// </summary>
        public RepoEndpoint Repo
        {
            get;
            private set;
        }


        internal HttpClient HttpClient
        {
            get;
            private set;
        }

        internal BlueskyServiceInfo ServiceInfo
        {
            get;
            private set;
        }

        internal BlueskySessionInfo SessionInfo
        {
            get;
            private set;
        }


        private BlueskyService(BlueskyServiceInfo svInfo, BlueskyAuthType authType)
        {
            this._isDisposed = false;
            this.HttpClient = new HttpClient();
            this.AuthType = authType;
            this.JwtAutoRefresh = true;
            this.ServiceInfo = svInfo;

            this.Server = new ServerEndpoint(this);
            this.Server.Calling += this._onApiCalling;

            this.Repo = new RepoEndpoint(this);
            this.Repo.Calling += this._onApiCalling;
        }


        private void _onApiCalling(Object sender, EventArgs e)
        {
            if (!this.JwtAutoRefresh)
                return;

            var tokenExpiration = this.SessionInfo.AccessJwtExpiration;
            if (tokenExpiration - DateTimeOffset.Now > TimeSpan.FromMinutes(10))
                return;

            this.RefreshJwtAsync().Wait();
        }

        /// <summary>
        /// Refreshes the JWT.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task RefreshJwtAsync()
        {
            var refTokenExpiration = this.SessionInfo.RefreshJwtExpiration;
            if (refTokenExpiration - DateTimeOffset.Now < TimeSpan.FromSeconds(30))
                throw new InvalidOperationException("The Refresh JWT has expired. Please request a new token.");

            var refreshResult = await this.Server.RefreshSessionAsync();
            this.SessionInfo.AccessJwt = refreshResult.AccessJwt;
            this.SessionInfo.RefreshJwt = refreshResult.RefreshJwt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BlueskySessionInfo GetSessionInfo()
        {
            return this.SessionInfo.GetClone();
        }

        /// <summary>
        /// Disposes of all resources used by the instance.
        /// </summary>
        public void Dispose()
        {
            if (this._isDisposed)
                return;

            this.HttpClient?.Dispose();
            this._isDisposed = true;
        }

        /// <summary>
        /// Logs in to the service using <see cref="BlueskyLoginInfo"/>.
        /// </summary>
        /// <param name="svInfo">Information about the target service.</param>
        /// <param name="loginInfo"></param>
        /// <returns></returns>
        public static async Task<BlueskyService> LoginWithLoginInfoAsync(BlueskyServiceInfo svInfo, BlueskyLoginInfo loginInfo)
        {
            var instance = new BlueskyService(svInfo, BlueskyAuthType.HandleAndPassword);
            var createSessionResult = await instance.Server.CreateSessionAsync(loginInfo.ToCreateSessionParam());

            instance.SessionInfo = new BlueskySessionInfo()
            {
                AccessJwt = createSessionResult.AccessJwt,
                RefreshJwt = createSessionResult.RefreshJwt,
            };

            return instance;
        }
    }
}

