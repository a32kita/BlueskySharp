using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class ProcedureDefinition
    {
        public string Type
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public SchemaDefinition InputSchema
        {
            get;
            set;
        }

        public SchemaDefinition OutputSchema
        {
            get;
            set;
        }

        public ErrorDefinition[] Errors
        {
            get;
            set;
        }
    }
}
