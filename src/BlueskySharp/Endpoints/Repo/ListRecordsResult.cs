using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Endpoints.Repo
{
    public class ListRecordsResult
    {
        public Record[] Records
        {
            get;
            set;
        }

        public string Cursor
        {
            get;
            set;
        }

        public class Record
        {
            public string Uri
            {
                get;
                set;
            }

            public string Cid
            {
                get;
                set;
            }

            public Endpoints.Record Value
            {
                get;
                set;
            }
        }
    }
}
