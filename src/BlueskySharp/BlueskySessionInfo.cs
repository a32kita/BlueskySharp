using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp
{
    public class BlueskySessionInfo
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
    }
}
