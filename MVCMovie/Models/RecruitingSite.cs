using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel;

namespace MVCMovie.Models
{
    public class RecruitingSite
    {

        public int ID { get; set; }

        [StringLength(256)]
        [Required]
        public string url { get; set; }

        [DefaultValue(false)]
        public bool isContainJobLink { get; set; }

        [DefaultValue(0)]
        public int levelNoLinkHigherJob1 {get; set; }

        public virtual IList<NextPosition> ListNextPositions { get; set; }

        public virtual IList<PathNode> JobPath { get; set; }

        public virtual IList<Job2Position> Job2Path { get; set; }

        public virtual IList<Company> companyPath { get; set; }

        public virtual IList<Others> othersPath { get; set; }
    }

    public class RecruitingSiteDBContext : DbContext
    {
        public DbSet<RecruitingSite> RecruitingSites { get; set; }
        public DbSet<Condition> Conditions { get; set; }

        
      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
        //Specify one-many between Condition and titleConds, and enable cascade delete
        modelBuilder.Entity<Condition>().HasMany(i => i.titleConds).WithRequired()
            .WillCascadeOnDelete();

        modelBuilder.Entity<Condition>().HasMany(i => i.locationConds).WithRequired()
            .WillCascadeOnDelete();
      }


    }

}