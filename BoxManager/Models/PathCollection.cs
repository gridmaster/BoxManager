﻿using System.Collections.Generic;

namespace BoxManager.Models
{
    public class PathCollection
    {
        public int total_count { get; set; }
        public IList<Entry> entries { get; set; }
    }
}