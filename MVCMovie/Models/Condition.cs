using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MVCMovie.Models
{
    public class Condition
    {
        public int ID { get; set; }
        
        public IList<TitleCond> titleConds { get; set; }

        public IList<LocationCond> locationConds { get; set; }
    }
}