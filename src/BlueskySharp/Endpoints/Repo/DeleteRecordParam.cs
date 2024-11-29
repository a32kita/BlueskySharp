using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class DeleteRecordParam
    {
        /// <summary>
        /// The handle or DID of the repo (aka, current account).
        /// </summary>
        public string Repo
        {
            get;
            set;
        }

        /// <summary>
        /// The NSID of the record collection.
        /// </summary>
        public string Collection
        {
            get;
            set;
        }

        /// <summary>
        /// The Record Key.
        /// </summary>
        public string Rkey
        {
            get;
            set;
        }

        /// <summary>
        /// Compare and swap with the previous record by CID.
        /// </summary>
        public string SwapRecord
        {
            get;
            set;
        }

        /// <summary>
        /// Compare and swap with the previous commit by CID.
        /// </summary>
        public string SwapCommit
        {
            get;
            set;
        }
    }
}
