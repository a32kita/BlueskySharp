using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class ListRecordsParam
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
        /// The NSID of the record type.
        /// </summary>
        public string Collection
        {
            get;
            set;
        }

        /// <summary>
        /// The number of records to return. (Minimum: 1, Maximum: 100, Default: 50)
        /// </summary>
        public int Limit
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// Flag to reverse the order of the returned records.
        /// </summary>
        public bool Reverse
        {
            get;
            set;
        }
    }
}
