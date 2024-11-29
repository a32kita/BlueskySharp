using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.BskyActor
{
    public class GetProfileParameter
    {
        /// <summary>
        /// Handle or DID of account to fetch profile of.
        /// </summary>
        public string Actor
        {
            get;
            set;
        }
    }
}
