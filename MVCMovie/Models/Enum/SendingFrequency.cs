using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVCMovie.Resources;

namespace MVCMovie.Models.Enum
{
    public enum SendingFrequency
    {
        [Display(Name = "Daily", ResourceType = typeof(Resource))]
        Daily = 1,

        [Display(Name = "EveryOtherDay", ResourceType = typeof(Resource))]
        EveryOtherDay = 2,

        [Display(Name = "Weekly", ResourceType = typeof(Resource))]
        Weekly = 3
    }
}