﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlueskySharp.EndPoints
{
    public class AttachedImage
    {
        public string Alt
        {
            get;
            set;
        }

        public AspectRatio AspectRatio
        {
            get;
            set;
        }

        public Blob Image
        {
            get;
            set;
        }
    }
}
