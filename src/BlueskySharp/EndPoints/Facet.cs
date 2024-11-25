using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.EndPoints
{
    public class Facet
    {
        public Feature[] Features
        {
            get;
            set;
        }

        public FacetIndex Index
        {
            get;
            set;
        }
    }
}
