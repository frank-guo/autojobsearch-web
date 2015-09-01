using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVCMovie.Resources;
using MVCMovie.Models.Enum;
using System.Data.Entity;

namespace MVCMovie.Models
{
    public class Email
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("site")]
        public int ID { get; set; }

        [StringLength(256)]
        [Required]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resource))]
        public string address { get; set; }

        [StringLength(48)]
        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string password { get; set; }

        [Display(Name = "SendingFrequency", ResourceType = typeof(Resource))]
        public SendingFrequency frequecy { get; set; }

        [Display(Name = "SendingOn", ResourceType = typeof(Resource))]
        public bool sendingOn { get; set; }

        public virtual RecruitingSite site { get; set; }
    }

}