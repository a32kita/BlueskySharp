using BlueskySharp.Models.Server;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueskySharp.EndPoints.Server
{
    public class ServerEndpoint : EndPointBase
    {
        internal ServerEndpoint(BlueskyService parent)
            : base(parent)
        {
            // NOP
        }

        public async Task<CreateSessionResult> CreateSessionAsync(CreateSessionParam param)
        {
            return await this.InvokeProcedureAsync<CreateSessionParam, CreateSessionResult>("xrpc/com.atproto.server.createSession", param, false);
        }
    }
}
