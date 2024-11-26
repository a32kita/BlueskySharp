using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class GetRecordResult
    {
        public string Uri
        {
            get;
            set;
        }

        public string Cid
        {
            get;
            set;
        }

        public Record Value
        {
            get;
            set;
        }
    }
}
