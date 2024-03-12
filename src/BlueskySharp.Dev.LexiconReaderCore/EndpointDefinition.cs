using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class EndpointDefinition
    {
        public uint Lexicon
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public ProcedureDefinition Procedure
        {
            get;
            set;
        }

        public SchemaDefinition[] Objects
        {
            get;
            set;
        }
    }
}
