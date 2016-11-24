namespace MVCMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        location_ID = c.Int(),
                        title_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LocationConds", t => t.location_ID)
                .ForeignKey("dbo.RecruitingSites", t => t.ID)
                .ForeignKey("dbo.TitleConds", t => t.title_ID)
                .Index(t => t.ID)
                .Index(t => t.location_ID)
                .Index(t => t.title_ID);
            
            CreateTable(
                "dbo.LocationConds",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        locationCond = c.String(maxLength: 60),
                        Condition_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Conditions", t => t.Condition_ID, cascadeDelete: true)
                .Index(t => t.Condition_ID);
            
            CreateTable(
                "dbo.RecruitingSites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        url = c.String(nullable: false, maxLength: 256),
                        isContainJobLink = c.Boolean(nullable: false),
                        levelNoLinkHigherJob1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        position = c.Int(nullable: false),
                        RecruitingSite_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSite_ID)
                .Index(t => t.RecruitingSite_ID);
            
            CreateTable(
                "dbo.Emails",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        address = c.String(nullable: false, maxLength: 256),
                        password = c.String(nullable: false, maxLength: 32),
                        frequency = c.Int(nullable: false),
                        sendingOn = c.Boolean(nullable: false),
                        smtpAddress = c.String(nullable: false),
                        smtpPort = c.Int(nullable: false),
                        sendingTime = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Job2Position",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        position = c.Int(nullable: false),
                        RecruitingSite_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSite_ID)
                .Index(t => t.RecruitingSite_ID);
            
            CreateTable(
                "dbo.PathNodes",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        RecruitingSite_ID = c.Int(nullable: false),
                        position = c.Int(nullable: false),
                        hasCommonParent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ID, t.RecruitingSite_ID })
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSite_ID, cascadeDelete: true)
                .Index(t => t.RecruitingSite_ID);
            
            CreateTable(
                "dbo.NextPositions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        position = c.Int(nullable: false),
                        RecruitingSite_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSite_ID)
                .Index(t => t.RecruitingSite_ID);
            
            CreateTable(
                "dbo.Others",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        position = c.Int(nullable: false),
                        RecruitingSite_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSite_ID)
                .Index(t => t.RecruitingSite_ID);
            
            CreateTable(
                "dbo.TitleConds",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        titleCond = c.String(maxLength: 60),
                        Condition_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Conditions", t => t.Condition_ID, cascadeDelete: true)
                .Index(t => t.Condition_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TitleConds", "Condition_ID", "dbo.Conditions");
            DropForeignKey("dbo.Conditions", "title_ID", "dbo.TitleConds");
            DropForeignKey("dbo.Conditions", "ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.Others", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.NextPositions", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.PathNodes", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.Job2Position", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.Emails", "ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.Companies", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.LocationConds", "Condition_ID", "dbo.Conditions");
            DropForeignKey("dbo.Conditions", "location_ID", "dbo.LocationConds");
            DropIndex("dbo.TitleConds", new[] { "Condition_ID" });
            DropIndex("dbo.Others", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.NextPositions", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.PathNodes", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.Job2Position", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.Emails", new[] { "ID" });
            DropIndex("dbo.Companies", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.LocationConds", new[] { "Condition_ID" });
            DropIndex("dbo.Conditions", new[] { "title_ID" });
            DropIndex("dbo.Conditions", new[] { "location_ID" });
            DropIndex("dbo.Conditions", new[] { "ID" });
            DropTable("dbo.TitleConds");
            DropTable("dbo.Others");
            DropTable("dbo.NextPositions");
            DropTable("dbo.PathNodes");
            DropTable("dbo.Job2Position");
            DropTable("dbo.Emails");
            DropTable("dbo.Companies");
            DropTable("dbo.RecruitingSites");
            DropTable("dbo.LocationConds");
            DropTable("dbo.Conditions");
        }
    }
}
