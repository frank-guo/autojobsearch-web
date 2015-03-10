using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MVCMovie.Models
{
    public class RecruitingSite
    {

        public int ID { get; set; }

        [StringLength(256)]
        [Required]
        public string url { get; set; }

        public int levelNoLinkHigherJob1 {get; set; }

        public virtual IList<NextPosition> ListNextPositions { get; set; }

        public virtual IList<PathNode> JobPath { get; set; }

        public virtual IList<Company> companyPath { get; set; }

        public virtual IList<Others> othersPath { get; set; }
    }

    public class RecruitingSiteDBContext : DbContext
    {
        public DbSet<RecruitingSite> RecruitingSites { get; set; }
        public DbSet<Condition> Conditions { get; set; }
    }

}