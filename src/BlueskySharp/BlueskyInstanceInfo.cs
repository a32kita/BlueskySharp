using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eobw.BlueskySharp
{
    /// <summary>
    /// The information required to connect to the Bluesky server
    /// </summary>
    public class BlueskyInstanceInfo
    {
        private string[] _allowedUriPrefixes;


        /// <summary>
        /// Gets the domain of the instance.
        /// </summary>
        public string InstanceDomain
        {
            get;
            private set;
        }

        ///// <summary>
        ///// Gets the user handle.
        ///// </summary>
        //public string Handle
        //{
        //    get;
        //    private set;
        //}

        ///// <summary>
        ///// Gets the password.
        ///// </summary>
        //public string Password
        //{
        //    get;
        //    private set;
        //}

        /// <summary>
        /// Gets the prefix for HTTP.
        /// </summary>
        public string InstanceUriPrefix
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the root of the API endpoint.
        /// </summary>
        public string InstanceApiEndpointRoot
        {
            get;
            private set;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="BlueskyInstanceInfo"/> class from the specified Bluesky instance informations.
        /// </summary>
        /// <param name="instanceDomain">Domain of the instance (ex: "bsky.social")</param>
        /// <param name="handle">User handle (ex: "contoso.bsky.social")</param>
        /// <param name="password">Password</param>
        public BlueskyInstanceInfo(string instanceDomain, string handle, string password)
        {
            this._allowedUriPrefixes = new string[] { "https", "http" };

            this.InstanceDomain = instanceDomain;
            //this.Handle = handle;
            //this.Password = password;

            this.InstanceUriPrefix = this._allowedUriPrefixes[0];
            this.InstanceApiEndpointRoot = "xrpc";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlueskyInstanceInfo"/> class.
        /// </summary>
        public BlueskyInstanceInfo()
            : this(String.Empty, String.Empty, String.Empty)
        {
            // NOP
        }


        private string _buildUriStr()
        {
            var sb = new StringBuilder();
            
            sb.Append(this.InstanceUriPrefix);
            sb.Append("://");
            sb.Append(this.InstanceDomain);
            
            if (String.IsNullOrEmpty(this.InstanceApiEndpointRoot) == false)
            {
                sb.Append("/");
                sb.Append(this.InstanceApiEndpointRoot);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Sets the domain of the instance. (ex: "bsky.social")
        /// </summary>
        /// <param name="instanceDomain">Domain of the instance (ex: "bsky.social")</param>
        /// <returns>Instance of the <see cref="BlueskyInstanceInfo"/> class</returns>
        public BlueskyInstanceInfo SetInstaceDomain(string instanceDomain)
        {
            this.InstanceDomain = instanceDomain;
            return this;
        }

        ///// <summary>
        ///// Sets the user handle. (ex: "contoso.bsky.social")
        ///// </summary>
        ///// <param name="handle">User handle (ex: "contoso.bsky.social")</param>
        ///// <returns>Instance of the <see cref="BlueskyConnectionInfo"/> class</returns>
        //public BlueskyConnectionInfo SetHandle(string handle)
        //{
        //    this.Handle = handle;
        //    return this;
        //}

        ///// <summary>
        ///// Sets the password.
        ///// </summary>
        ///// <param name="password">Password</param>
        ///// <returns>Instance of the <see cref="BlueskyConnectionInfo"/> class</returns>
        //public BlueskyConnectionInfo SetPassword(string password)
        //{
        //    this.Password = password;
        //    return this;
        //}

        /// <summary>
        /// Sets the prefix for HTTP.
        /// </summary>
        /// <param name="instanceUriPrefix">Prefix for HTTP</param>
        /// <returns>Instance of the <see cref="BlueskyConnectionInfo"/> class</returns>
        /// <exception cref="NotSupportedException">The specified prefix is not supported.</exception>
        public BlueskyInstanceInfo SetInstanceUriPrefix(string instanceUriPrefix)
        {
            if (this._allowedUriPrefixes.Contains(instanceUriPrefix) == false)
                throw new NotSupportedException($"The specified prefix is not supported. Prefix '{instanceUriPrefix}' is not supported.");

            this.InstanceUriPrefix = instanceUriPrefix;
            return this;
        }

        /// <summary>
        /// Set the root of the API endpoint. Usually not used. (init: "xrpc")
        /// </summary>
        /// <param name="instanceApiEndpointRoot">Root of the API endpoint (init: "xrpc")</param>
        /// <returns>Instance of the <see cref="BlueskyInstanceInfo"/> class</returns>
        /// <exception cref="ArgumentException">An invalid path has been set.</exception>
        public BlueskyInstanceInfo SetInstanceApiEndpointRoot(string instanceApiEndpointRoot)
        {
            instanceApiEndpointRoot = instanceApiEndpointRoot.Trim('/');
            if (instanceApiEndpointRoot.Contains("../"))
                throw new ArgumentException($"An invalid path has been set. Invalid uri: {instanceApiEndpointRoot}", nameof(instanceApiEndpointRoot));

            this.InstanceApiEndpointRoot = instanceApiEndpointRoot;
            return this;
        }

        /// <summary>
        /// Verifies if sufficient information has been configured to connect to Bluesky.
        /// </summary>
        /// <returns>The result of the verification.</returns>
        public bool IsValid()
        {
            if (String.IsNullOrEmpty(this.InstanceDomain)) //||
                //String.IsNullOrEmpty(this.Handle) ||
                //String.IsNullOrEmpty(this.Password))
            {
                return false;
            }

            var uriStr = this._buildUriStr();
            Uri uri;
            return Uri.TryCreate(uriStr, UriKind.Absolute, out uri);
        }
    }
}
