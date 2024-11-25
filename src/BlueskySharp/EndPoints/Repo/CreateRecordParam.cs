using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class CreateRecordParam
    {
        public string Repo
        {
            get;
            set;
        }

        public string Collection
        {
            get;
            set;
        }

        public string Rkey
        {
            get;
            set;
        }

        public bool Validate
        {
            get;
            set;
        }

        public Record Record
        {
            get;
            set;
        }

        public string SwapCommit
        {
            get;
            set;
        }
    }
}

