using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data.Entity;
using System.ComponentModel;

namespace MVCMovie.Models
{
    public class RecruitingSite
    {
        public int ID { get; set; }

        [StringLength(256)]
        [Required]
        [Display(Name = "URL")]
        public string url { get; set; }

        [DefaultValue(false)]
        public bool isContainJobLink { get; set; }

        [DefaultValue(0)]
        public int levelNoLinkHigherJob1 {get; set; }

        [Display(Name = "Next Page")]
        public virtual IList<NextPosition> ListNextPositions { get; set; }

        [Display(Name = "Job1")]
        public virtual IList<PathNode> JobPath { get; set; }

        [Display(Name = "Job2")]
        public virtual IList<Job2Position> Job2Path { get; set; }

        [Display(Name = "Company")]
        public virtual IList<Company> companyPath { get; set; }

        [Display(Name = "Others")]
        public virtual IList<Others> othersPath { get; set; }

        public virtual Condition condition { get; set; }

        public virtual Email email { get; set; }
    }

    public class RecruitingSiteDBContext : DbContext
    {
        public DbSet<RecruitingSite> RecruitingSites { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Email> Emails { get; set; }

        
      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
        //Specify one-many between Condition and titleConds, and enable cascade delete
        modelBuilder.Entity<Condition>().HasMany(i => i.titleConds).WithRequired()
            .WillCascadeOnDelete();

        modelBuilder.Entity<Condition>().HasMany(i => i.locationConds).WithRequired()
            .WillCascadeOnDelete();

        modelBuilder.Entity<PathNode>()
            .HasKey(p => new { p.ID, p.RecruitingSite_ID });
        //modelBuilder.Entity<PathNode>().HasRequired(p => p.RecruitingSite).WithMany().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.Job2Path).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.ListNextPositions).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.companyPath).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.othersPath).WithRequired().WillCascadeOnDelete();
      }


    }

}