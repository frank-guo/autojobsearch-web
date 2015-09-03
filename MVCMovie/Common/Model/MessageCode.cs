using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVCMovie.Resources;

namespace MVCMovie.Common.Model
{
    public enum MessageCode
    {
        [Display(Name = "SaveSuccessMsg", ResourceType = typeof(Resources.Common))]
        Success = 0,

        [Display(Name = "InvlidSiteID", ResourceType = typeof(Resources.Common))]
        InvlidSiteID = 1,

        [Display(Name = "InvlidModel", ResourceType = typeof(Resources.Common))]
        InvlidModel = 2,
    }
}