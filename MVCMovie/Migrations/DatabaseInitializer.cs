using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCMovie.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MVCMovie.Migrations
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<MVCMovie.Models.ApplicationDbContext>
    {
        //This seed method is called when running the project at the first time
        //In other words, it is only called when the database doesn't exist at all before running the project.
        //In order to add some new seed data after the first time of running the project, use the seed method in Configuration.cs.
        protected override void Seed(MVCMovie.Models.ApplicationDbContext context)
        {
            //context.RecruitingSites.Add(
            //  new RecruitingSite { siteName = "T-Net", url = "http://www.bctechnology.com/jobs/search-results.cfm" }
            //);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role.Name = "Regular";
            roleManager.Create(role);
            
            role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role.Name = "Admin";
            roleManager.Create(role);
        }
    }
}