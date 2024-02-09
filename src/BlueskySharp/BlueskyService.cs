using System;
using System.Collections.Generic;
using System.Text;

namespace Eobw.BlueskySharp
{
    public class BlueskyService
    {
        private string _endpointRoot;
        private string _accessJwt;
        private string _refreshJwt;


        public BlueskyService(BlueskyInstanceInfo instanceInfo)
        {
            this._endpointRoot = instanceInfo.BuildEndpointRootUri();
        }
    }
}
