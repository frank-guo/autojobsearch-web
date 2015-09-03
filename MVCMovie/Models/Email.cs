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

        [StringLength(256, MinimumLength = 3)]
        [RegularExpression("^([a-zA-Z0-9_.+-])+@(([a-zA-Z0-9-])+.)+([a-zA-Z0-9]{2,4})+$", 
                ErrorMessageResourceType=typeof(Resources.Email),
                ErrorMessageResourceName= "InvalidEmailAddress")]
        [Required]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.Email))]
        public string address { get; set; }

        [StringLength(32)]
        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.Email))]
        public string password { get; set; }

        [Display(Name = "SendingFrequency", ResourceType = typeof(Resources.Email))]
        public SendingFrequency frequency { get; set; }

        [Display(Name = "SendingOn", ResourceType = typeof(Resources.Email))]
        public bool sendingOn { get; set; }

        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9-_]{0,61}[a-zA-Z0-9]{0,1}.([a-zA-Z]{1,6}|[a-zA-Z0-9-]{1,30}.[a-zA-Z]{2,3})$",
                ErrorMessageResourceType = typeof(Resources.Email),
                ErrorMessageResourceName = "InvalidSMTPAddress")]
        [Required]
        [Display(Name = "SMTPAddress", ResourceType = typeof(Resources.Email))]
        public string smtpAddress { get; set; }

        [RegularExpression("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$",
                ErrorMessageResourceType = typeof(Resources.Email),
                ErrorMessageResourceName = "InvalidPortNumber")]
        [Required]
        [Display(Name = "SMTPPort", ResourceType = typeof(Resources.Email))]
        public int smtpPort { get; set; }

        [Display(Name = "SendingTime", ResourceType = typeof(Resources.Email))]
        public string sendingTime { get; set; }

        public virtual RecruitingSite site { get; set; }
    }

}