using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Eobw.BlueskySharp
{
    public class BlueskyService : IDisposable
    {
        private bool _disposed;

        private string _endpointRoot;
        private string _accessJwt;
        private string _refreshJwt;

        private HttpClient _httpClient;


        public BlueskyService(BlueskyInstanceInfo instanceInfo)
        {
            this._endpointRoot = instanceInfo.BuildEndpointRootUri();
            this._initializeHttpClient();
        }


        private void _initializeHttpClient()
        {
            this._httpClient = new HttpClient();
        }


        public void Dispose()
        {
            if (this._disposed)
                return;

            this._httpClient?.Dispose();
            this._disposed = true;
        }
    }
}
