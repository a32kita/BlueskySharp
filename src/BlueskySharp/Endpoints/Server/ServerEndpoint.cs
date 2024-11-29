using BlueskySharp.Models.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueskySharp.Endpoints.Server
{
    public class ServerEndpoint : EndpointBase
    {
        internal ServerEndpoint(BlueskyService parent)
            : base(parent)
        {
            // NOP
        }

        /// <summary>
        /// Create an authentication session.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<CreateSessionResult> CreateSessionAsync(CreateSessionParam param)
        {
            return await this.InvokeProcedureAsync<CreateSessionParam, CreateSessionResult>("xrpc/com.atproto.server.createSession", param, false, null, true);
        }

        /// <summary>
        /// Refresh an authentication session. Requires auth using the 'refreshJwt' (not the 'accessJwt').
        /// </summary>
        /// <returns></returns>
        public async Task<RefreshSessionResult> RefreshSessionAsync()
        {
            return await this.InvokeProcedureAsync<EmptyParam, RefreshSessionResult>("xrpc/com.atproto.server.refreshSession", EmptyParam.Instance, false, this.SessionInfo.RefreshJwt, true);
        }

        /// <summary>
        /// Delete the current session. Requires auth.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteSessionAsync()
        {
            await this.InvokeProcedureAsync<EmptyParam, EmptyResult>("xrpc/com.atproto.server.deleteSession", EmptyParam.Instance, false, this.SessionInfo.RefreshJwt, false);
        }
    }
}
