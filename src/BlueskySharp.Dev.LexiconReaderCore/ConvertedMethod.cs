using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class ConvertedMethod
    {
        public string Name
        {
            get;
            set;
        }

        public string Summary
        {
            get;
            set;
        }

        public string EndpointName
        {
            get;
            set;
        }

        public ConvertedParameter[] Parameters
        {
            get;
            set;
        }

        public ConvertedEntity ReturnValue
        {
            get;
            set;
        }
    }
}
