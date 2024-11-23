using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using BlueskySharp.Models.Server;

namespace BlueskySharp
{
    public class BlueskyLoginInfo
    {
        public string Handle
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public CreateSessionParam ToCreateSessionParam()
        {
            return new CreateSessionParam()
            {
                Identifier = this.Handle,
                Password = this.Password,
            };
        }
    }
}
