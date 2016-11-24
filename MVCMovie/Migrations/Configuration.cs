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

        protected override void Seed(MVCMovie.Models.RecruitingSiteDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.RecruitingSites.AddOrUpdate(
              site => site.siteName,
              new RecruitingSite { siteName = "T-Net", url = "http://www.bctechnology.com/jobs/search-results.cfm" }
            );
            //
        }
    }
}
