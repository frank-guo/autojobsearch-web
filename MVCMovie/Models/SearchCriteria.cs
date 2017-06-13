using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MVCMovie.Models
{
    public class SearchCriteria
    {

        public int ID { get; set; }

        [ForeignKey("Site")]
        public int RecruitingSiteId { get; set; }

        public string FieldName { get; set; }

        public string CriteriaOperator { get; set; }

        public virtual IList<SearchCriteriaValue> Values { get; set; }

        public virtual RecruitingSite Site { get; set; }
    }
}