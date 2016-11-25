namespace MVCMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecruitingSiteSiteName : DbMigration
    {
        public override void Up()
        {
            AddColumn("RecruitingSites", "siteName", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("RecruitingSites", "siteName");
        }
    }
}
