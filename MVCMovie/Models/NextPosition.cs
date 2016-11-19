using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCMovie.Models
{
    public class NextPosition
    {
        public int ID { get; set; }
        public int position {get; set;}

        //[Column("RecruitingSite_ID")]
        //public int RecruitingSiteId { get; set; }

        //[ForeignKey("RecruitingSiteId")]
        //public virtual RecruitingSite RecruitingSite { get; set; }
    }
}