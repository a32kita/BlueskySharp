using System;
using System.Collections.Generic;
using System.Text;

namespace Eobw.BlueskySharp
{
    public class BlueskyLegacyAuthenticationInfo
    {
        /// <summary>
        /// Gets the user handle.
        /// </summary>
        public string Handle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password
        {
            get;
            private set;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BlueskyLegacyAuthenticationInfo"/> class from the specified Bluesky authentication informations.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="password"></param>
        public BlueskyLegacyAuthenticationInfo(string handle, string password)
        {
            this.Handle = handle;
            this.Password = password;
        }

        public BlueskyLegacyAuthenticationInfo()
            : this(String.Empty, String.Empty)
        {
            // NOP
        }


        /// <summary>
        /// Sets the user handle. (ex: "contoso.bsky.social")
        /// </summary>
        /// <param name="handle">User handle (ex: "contoso.bsky.social")</param>
        /// <returns>Instance of the <see cref="BlueskyLegacyAuthenticationInfo"/> class</returns>
        public BlueskyLegacyAuthenticationInfo SetHandle(string handle)
        {
            this.Handle = handle;
            return this;
        }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Instance of the <see cref="BlueskyLegacyAuthenticationInfo"/> class</returns>
        public BlueskyLegacyAuthenticationInfo SetPassword(string password)
        {
            this.Password = password;
            return this;
        }
    }
}
