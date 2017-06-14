using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MVCMovie.Models
{
    public class SearchCriteriaValue
    {
        public int ID { get; set; }

        [ForeignKey("SearchCriteria")]
        public int SearchCriteriaId { get; set; }

        public string value { get; set; }

        public virtual SearchCriteria SearchCriteria { get; set; }
    }
}