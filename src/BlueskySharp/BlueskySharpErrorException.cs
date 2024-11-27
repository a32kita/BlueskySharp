using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp
{
    public class BlueskySharpErrorException : Exception
    {
        public string SourceJson
        {
            get;
            private set;
        }

        public string Error
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }

        public BlueskySharpErrorException(string sourceJson, string error, string errorMessage)
            : base($"Bluesky Error '{error}': {errorMessage}")
        {
            this.SourceJson = sourceJson;
            this.Error = error;
            this.ErrorMessage = errorMessage;
        }

        public BlueskySharpErrorException(string sourceJson, Endpoints.ErrorResult errorResult)
            : this(sourceJson, errorResult.Error, errorResult.Message)
        {
            // NOP
        }
    }
}
