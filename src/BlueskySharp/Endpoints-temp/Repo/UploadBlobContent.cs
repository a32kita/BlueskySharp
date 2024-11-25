using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class UploadBlobContent : IDisposable
    {
        private bool _leaveOpen;
        private bool _isDisposed;


        public string MimeType
        {
            get;
            private set;
        }

        public Stream ContentStream
        {
            get;
            private set;
        }

        public UploadBlobContent(string mimeType, Stream contentStream, bool leaveOpen = false)
        {
            this._isDisposed = false;

            this.MimeType = mimeType;
            this.ContentStream = contentStream;
            this._leaveOpen = leaveOpen;
        }


        public void Dispose()
        {
            if (this._isDisposed)
                return;

            if (!this._leaveOpen)
                this.ContentStream.Dispose();

            this.ContentStream = null;
            this._isDisposed = true;
        }
    }
}

