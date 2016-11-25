namespace MVCMovie.Migrations
{
    using MVCMovie.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCMovie.Models.RecruitingSiteDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "MVCMovie.Models.RecruitingSiteDBContext";
        }

        protected override void Seed(RecruitingSiteDBContext context)
        {
            context.RecruitingSites.Add(
              new RecruitingSite { siteName = "T-Net", url = "http://www.bctechnology.com/jobs/search-results.cfm" }
            );
        }
    }
}
