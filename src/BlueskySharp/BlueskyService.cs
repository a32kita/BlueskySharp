using BlueskySharp.EndPoints.Repo;
using BlueskySharp.EndPoints.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlueskySharp
{
    public class BlueskyService : IDisposable
    {
        private bool _isDisposed;

        public BlueskyAuthType AuthType
        {
            get;
            private set;
        }

        public ServerEndpoint Server
        {
            get;
            private set;
        }

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
            this.ServiceInfo = svInfo;

            this.Server = new ServerEndpoint(this);
            this.Repo = new RepoEndpoint(this);
        }


        public void Dispose()
        {
            if (this._isDisposed)
                return;

            this.HttpClient?.Dispose();
            this._isDisposed = true;
        }


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
