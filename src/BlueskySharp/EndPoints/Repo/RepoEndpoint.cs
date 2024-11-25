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

        public async Task<ApplyWritesResult> ApplyWrites(ApplyWritesParam param)
        {
            return await this.InvokeProcedureAsync<ApplyWritesParam, ApplyWritesResult>("xrpc/com.atproto.repo.applyWrites", param);
        }

        /// <summary>
        /// Create a single new repository record. Requires auth, implemented by PDS.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<CreateRecordResult> CreateRecordAsync(CreateRecordParam param)
        {
            return await this.InvokeProcedureAsync<CreateRecordParam, CreateRecordResult>("xrpc/com.atproto.repo.createRecord", param);
        }

        /// <summary>
        /// Upload a new blob, to be referenced from a repository record. The blob will be deleted if it is not referenced within a time window (eg, minutes). Blob restrictions (mimetype, size, etc) are enforced when the reference is created. Requires auth, implemented by PDS.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<UploadBlobResult> UploadBlobAsync(UploadBlobContent content)
        {
            return await this.UploadContentAsync<UploadBlobResult>("xrpc/com.atproto.repo.uploadBlob", content.MimeType, content.ContentStream);
        }
    }
}
