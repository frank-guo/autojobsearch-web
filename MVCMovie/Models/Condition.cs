using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MVCMovie.Models
{
    public class Condition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        
        public virtual IList<TitleCond> titleConds { get; set; }

        public virtual IList<LocationCond> locationConds { get; set; }
    }
}