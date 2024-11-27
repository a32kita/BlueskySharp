using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints
{
    /// <summary>
    /// Represents an error returned by the server.
    /// </summary>
    public class ErrorResult
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string Error
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message
        {
            get;
            set;
        }
    }
}
