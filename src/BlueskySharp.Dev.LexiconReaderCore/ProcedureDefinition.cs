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

        public ProcedureIODefinition Input
        {
            get;
            set;
        }

        public ProcedureIODefinition Output
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
