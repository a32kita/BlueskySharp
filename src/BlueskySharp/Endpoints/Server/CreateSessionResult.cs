using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Models.Server
{
    public class CreateSessionResult
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

        public string Email
        {
            get;
            set;
        }

        public bool EmailConfirmed
        {
            get;
            set;
        }

        public bool EmailAuthFactor
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        /// <summary>
        /// If active=false, this optional field indicates a possible reason for why the account is not active. If active=false and no status is supplied, then the host makes no claim for why the repository is no longer being hosted.
        /// </summary>
        public CreateSessionResult_Status Status
        {
            get;
            set;
        }
    }
}
