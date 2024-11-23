using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Models.Server
{
    public class CreateSessionParam
    {
        /// <summary>
        /// Handle or other identifier supported by the server for the authenticating user.
        /// </summary>
        public string Identifier
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
