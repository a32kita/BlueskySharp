using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Eobw.BlueskySharp.Endpoints
{
    public class BlueskyEndpointBase
    {
        public BlueskyService Parent
        {
            get;
            private set;
        }

        internal BlueskyEndpointBase(BlueskyService parent)
        {
            this.Parent = parent;
        }
    }
}
