using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Models
{
    public class ConditionViewModel
    {
        public List<string> titleConds { get; set; }
        public List<string> locationConds { get; set; }
    }
}