using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp
{
    /// <summary>
    /// An exception that represents a failure in the API.
    /// </summary>
    public class BlueskySharpErrorException : Exception
    {
        /// <summary>
        /// Retrieves the raw JSON data returned by the server.
        /// </summary>
        public string SourceJson
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string Error
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// New instance of <see cref="BlueskySharpErrorException"/> class.
        /// </summary>
        /// <param name="sourceJson"></param>
        /// <param name="error"></param>
        /// <param name="errorMessage"></param>
        public BlueskySharpErrorException(string sourceJson, string error, string errorMessage)
            : base($"Bluesky Error '{error}': {errorMessage}")
        {
            this.SourceJson = sourceJson;
            this.Error = error;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// New instance of <see cref="BlueskySharpErrorException"/> class.
        /// </summary>
        /// <param name="sourceJson"></param>
        /// <param name="errorResult"></param>
        public BlueskySharpErrorException(string sourceJson, Endpoints.ErrorResult errorResult)
            : this(sourceJson, errorResult.Error, errorResult.Message)
        {
            // NOP
        }
    }
}
