using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.Dev.LexiconReaderCore
{
    public class ConvertedParameter
    {
        public string Name
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public bool Required
        {
            get;
            set;
        }

        public string Summary
        {
            get;
            set;
        }

        public bool JsonConvertionUnsupported
        {
            get;
            set;
        }
    }
}
