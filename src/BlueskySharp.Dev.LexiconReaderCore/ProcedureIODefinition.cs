using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class ProcedureIODefinition
    {
        public string Encoding
        {
            get;
            set;
        }

        //public string Type
        //{
        //    get;
        //    set;
        //}

        //public string[] Required
        //{
        //    get;
        //    set;
        //}

        //public PropertyDefinition[] Properties
        //{
        //    get;
        //    set;
        //}

        public SchemaDefinition Schema
        {
            get;
            set;
        }
    }
}
