using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMovie.Models
{
    public class SearchCriteriaViewModel
    {
        public int id { get; set; }
        public int recruitingSiteId { get; set; }
        public string[] fieldName { get; set; }
        public string _operator { get; set; }
        public virtual IList<string> values { get; set; }
    }
}