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
        [ForeignKey("site")]
        public int ID { get; set; }

        [Display(Name = "Job Title")]
        public virtual IList<TitleCond> titleConds { get; set; }

        [Display(Name = "Location")]
        public virtual IList<LocationCond> locationConds { get; set; }

        public virtual RecruitingSite site { get; set; }
    }
}