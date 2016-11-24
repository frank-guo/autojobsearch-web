using MVCMovie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCMovie.Migrations
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<RecruitingSiteDBContext>
    {
        protected override void Seed(RecruitingSiteDBContext context)
        {
            context.RecruitingSites.Add(
              new RecruitingSite { siteName = "T-Net", url = "http://www.bctechnology.com/jobs/search-results.cfm" }
            );
        }
    }
}