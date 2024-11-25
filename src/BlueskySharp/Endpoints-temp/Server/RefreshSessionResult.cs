using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Server
{
    public class RefreshSessionResult
    {
        public string AccessJwt
        {
            get;
            set;
        }

        public string RefreshJwt
        {
            get;
            set;
        }

        public string Handle
        {
            get;
            set;
        }

        public string Did
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }
    }
}

