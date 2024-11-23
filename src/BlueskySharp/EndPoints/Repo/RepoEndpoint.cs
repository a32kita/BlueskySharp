using BlueskySharp.Models.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueskySharp.EndPoints.Repo
{
    public class RepoEndpoint : EndPointBase
    {
        internal RepoEndpoint(BlueskyService parent)
            : base(parent)
        {
            // NOP
        }

        public async Task<CreateRecordResult> CreateRecordAsync(CreateRecordParam param)
        {
            return await this.InvokeProcedureAsync<CreateRecordParam, CreateRecordResult>("xrpc/com.atproto.repo.createRecord", param);
        }
    }
}
