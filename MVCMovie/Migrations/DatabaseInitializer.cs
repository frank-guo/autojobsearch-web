﻿using MVCMovie.Models;
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
        protected override void Seed(MVCMovie.Models.ApplicationDbContext context)
        {
            //context.RecruitingSites.Add(
            //  new RecruitingSite { siteName = "T-Net", url = "http://www.bctechnology.com/jobs/search-results.cfm" }
            //);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            role.Name = "Regular";
            roleManager.Create(role); 
        }
    }
}