using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data.Entity;
using System.ComponentModel;
using Microsoft.AspNet.Identity.EntityFramework;

using System.Windows.Forms;

namespace MVCMovie.Models
{
    public class RecruitingSite
    {
        public int ID { get; set; }

        [StringLength(256)]
        [Required]
        [Display(Name = "Site Name")]
        public string siteName { get; set; }

        [StringLength(256)]
        [Required]
        [Display(Name = "URL")]
        public string url { get; set; }

        [DefaultValue(false)]
        public bool isContainJobLink { get; set; }

        [DefaultValue(0)]
        public int levelNoLinkHigherJob1 {get; set; }

        [ForeignKey("ApplicationUser")]
        public String ApplicationUser_Id { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Next Page")]
        public virtual IList<NextPosition> ListNextPositions { get; set; }

        [Display(Name = "First Job Title")]
        public virtual IList<PathNode> JobPath { get; set; }

        [Display(Name = "Second Job Title")]
        public virtual IList<Job2Position> Job2Path { get; set; }

        [Display(Name = "Company Information")]
        public virtual IList<Company> companyPath { get; set; }

        [Display(Name = "Other Information")]
        public virtual IList<Others> othersPath { get; set; }

        public virtual IList<SearchCriteria> SearchRule { get; set; }

        public virtual Condition condition { get; set; }

        public virtual Email email { get; set; }

        //Given the common ancestor of job1, company and others, Get the Node of this job title
        public HtmlElement getJobTitleNode(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<PathNode> jobPath = getJobPath();

            if (jobPath.Count == 0)
            {
                return null;
            }

            int idxCommonAnstr = (jobPath.Count - 1) - levelNoCommonAnstr;  //To get the index of common anstr, (jobPath.Count -1) has to be used here
                                                                            //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[jobPath[i].position];
            }

            return node;
        }

        private List<PathNode> getJobPath()
        {
            List<PathNode> JobPath = new List<PathNode>();

            foreach (PathNode p in JobPath)
            {
                JobPath.Add(p);
            }

            return JobPath;
        }

        //Given the common ancestor of job1, company and others, Get the other info of this job
        public HtmlElement getOtherInfo(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<Others> othersPath = getOthers();

            if (othersPath.Count == 0)
            {
                return null;
            }

            int idxCommonAnstr = (othersPath.Count - 1) - levelNoCommonAnstr;   //To get the index of common anstr, (othersPath.Count -1) has to be used here
                                                                                //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[othersPath[i].position];
            }

            return node;
        }

        private List<Others> getOthers()
        {
            List<Others> othersPath = new List<Others>();
            foreach (Others p in othersPath)
            {
                othersPath.Add(p);
            }

            return othersPath;
        }

        //Given the common ancestor of job1, company and others, Get the Node of this job company
        public HtmlElement getCompanyNameNode(HtmlElement commonAncestor, int levelNoCommonAnstr)
        {
            List<Company> companyPath = getCompanyPath();

            if (companyPath.Count == 0)
            {
                return null;
            }

            int idxCommonAnstr = (companyPath.Count - 1) - levelNoCommonAnstr;  //To get the index of common anstr, (companyPath.Count -1) has to be used here
                                                                                //since levelNoCommonAnstr actually starts from 0, like an index

            var node = commonAncestor;
            HtmlElementCollection children;

            for (int i = idxCommonAnstr - 1; i >= 0; i--)
            {
                children = node.Children;
                node = children[companyPath[i].position];
            }

            return node;
        }

        private List<Company> getCompanyPath()
        {
            List<Company> companyPath = new List<Company>();

            foreach (Company p in companyPath)
            {
                companyPath.Add(p);
            }

            return companyPath;
        }
    }

    public class RecruitingSiteDBContext : IdentityDbContext
    {
        public DbSet<RecruitingSite> RecruitingSites { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<Email> Emails { get; set; }

        
      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
          base.OnModelCreating(modelBuilder);

          //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
          //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
          //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

        //Specify one-many between Condition and titleConds, and enable cascade delete
        modelBuilder.Entity<Condition>().HasMany(i => i.titleConds).WithRequired()
            .WillCascadeOnDelete();

        modelBuilder.Entity<Condition>().HasMany(i => i.locationConds).WithRequired()
            .WillCascadeOnDelete();

        //modelBuilder.Entity<RecruitingSite>().HasRequired<ApplicationUser>(au => au.ApplicationUser)
        //    .WithMany(au => au.RecruitingSites).HasForeignKey(site => site.ApplicationUser_Id);

        //modelBuilder.Entity<PathNode>()
        //    .HasKey(p => new { p.ID, p.RecruitingSite_ID });
        //modelBuilder.Entity<PathNode>().HasRequired(p => p.RecruitingSite).WithMany().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.JobPath).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.Job2Path).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.ListNextPositions).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.companyPath).WithRequired().WillCascadeOnDelete();
        //modelBuilder.Entity<RecruitingSite>().HasMany(i => i.othersPath).WithRequired().WillCascadeOnDelete();
      }
    }

}