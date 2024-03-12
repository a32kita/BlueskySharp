using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class ConvertedEntity
    {
        public string TypeName
        {
            get;
            set;
        }

        public ConvertedProperty[] Properties
        {
            get;
            set;
        }
    }
}
