using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Models
{
    public class PathNodeViewModel
    {
            public int ID { get; set; }
            public int position { get; set; }
            public bool hasCommonParent { get; set; }
    }
}