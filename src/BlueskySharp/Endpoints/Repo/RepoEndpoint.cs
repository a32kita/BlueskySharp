using BlueskySharp.Models.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueskySharp.Endpoints.Repo
{
    /// <summary>
    /// Repo endpoint
    /// </summary>
    public class RepoEndpoint : EndpointBase
    {
        internal RepoEndpoint(BlueskyService parent)
            : base(parent)
        {
            // NOP
        }

        /// <summary>
        /// Apply a batch transaction of repository creates, updates, and deletes. Requires auth, implemented by PDS.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
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

        ///// <summary>
        ///// Delete a repository record, or ensure it doesn't exist. Requires auth, implemented by PDS.
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //public async Task<CommitMeta> DeleteRecordAsync(DeleteRecordParam param)
        //{
        //    return await this.InvokeProcedureAsync<DeleteRecordParam, CommitMeta>("xrpc/com.atproto.repo.deleteRecord", param);
        //}

        /// <summary>
        /// Get a single record from a repository. Does not require auth.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<GetRecordResult> GetRecordAsync(GetRecordParam param)
        {
            return await this.ExecuteQuery<GetRecordParam, GetRecordResult>("xrpc/com.atproto.repo.getRecord", param);
        }

        /// <summary>
        /// List a range of records in a repository, matching a specific collection. Does not require auth.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ListRecordsResult> ListRecordsAsync(ListRecordsParam param)
        {
            return await this.ExecuteQuery<ListRecordsParam, ListRecordsResult>("xrpc/com.atproto.repo.listRecords", param);
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


        public class CommitMeta
        {
            public string Cid
            {
                get;
                set;
            }

            public string Rev
            {
                get;
                set;
            }
        }
    }
}


