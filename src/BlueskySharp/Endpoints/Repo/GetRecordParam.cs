using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class GetRecordParam
    {
        /// <summary>
        /// The handle or DID of the repo.
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
        /// The CID of the version of the record. If not specified, then return the most recent version.
        /// </summary>
        public string Rkey
        {
            get;
            set;
        }
    }
}
