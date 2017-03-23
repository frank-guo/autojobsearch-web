namespace MVCMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdInRecruitingSite : DbMigration
    {
        public override void Up()
        {           
            AddColumn("dbo.RecruitingSites", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.RecruitingSites", "ApplicationUser_Id");
            AddForeignKey("dbo.RecruitingSites", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecruitingSites", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.RecruitingSites", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.RecruitingSites", "ApplicationUser_Id");
        }
    }
}
