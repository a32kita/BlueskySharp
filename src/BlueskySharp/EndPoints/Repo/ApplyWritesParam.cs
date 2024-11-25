using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class ApplyWritesParam
    {
        public string Repo
        {
            get;
            set;
        }

        public bool Validate
        {
            get;
            set;
        }

        public Write[] Writes
        {
            get;
            set;
        }
    }
}

