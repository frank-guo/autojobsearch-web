using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCMovie.Models
{
    public class Job2Position
    {
        public int ID { get; set; }
        public int position { get; set; }

        //[Column("RecruitingSite_ID")]
        //public int RecruitingSiteId { get; set; }

        //[ForeignKey("RecruitingSiteId")]
        //public virtual RecruitingSite RecruitingSite { get; set; }
    }
}