namespace MVCMovie.Migrations.ApplicationDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RecruitingSites",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        siteName = c.String(nullable: false, maxLength: 256),
                        url = c.String(nullable: false, maxLength: 256),
                        isContainJobLink = c.Boolean(nullable: false),
                        levelNoLinkHigherJob1 = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
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
                        Condition_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Conditions", t => t.Condition_ID)
                .Index(t => t.Condition_ID);
            
            CreateTable(
                "dbo.TitleConds",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        titleCond = c.String(maxLength: 60),
                        Condition_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Conditions", t => t.Condition_ID)
                .Index(t => t.Condition_ID);
            
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
                        ID = c.Int(nullable: false, identity: true),
                        position = c.Int(nullable: false),
                        hasCommonParent = c.Boolean(nullable: false),
                        RecruitingSite_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSite_ID)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Others", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.NextPositions", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.PathNodes", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.Job2Position", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.Emails", "ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.TitleConds", "Condition_ID", "dbo.Conditions");
            DropForeignKey("dbo.Conditions", "title_ID", "dbo.TitleConds");
            DropForeignKey("dbo.Conditions", "ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.LocationConds", "Condition_ID", "dbo.Conditions");
            DropForeignKey("dbo.Conditions", "location_ID", "dbo.LocationConds");
            DropForeignKey("dbo.Companies", "RecruitingSite_ID", "dbo.RecruitingSites");
            DropForeignKey("dbo.RecruitingSites", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.Others", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.NextPositions", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.PathNodes", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.Job2Position", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.Emails", new[] { "ID" });
            DropIndex("dbo.TitleConds", new[] { "Condition_ID" });
            DropIndex("dbo.LocationConds", new[] { "Condition_ID" });
            DropIndex("dbo.Conditions", new[] { "title_ID" });
            DropIndex("dbo.Conditions", new[] { "location_ID" });
            DropIndex("dbo.Conditions", new[] { "ID" });
            DropIndex("dbo.Companies", new[] { "RecruitingSite_ID" });
            DropIndex("dbo.RecruitingSites", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.Others");
            DropTable("dbo.NextPositions");
            DropTable("dbo.PathNodes");
            DropTable("dbo.Job2Position");
            DropTable("dbo.Emails");
            DropTable("dbo.TitleConds");
            DropTable("dbo.LocationConds");
            DropTable("dbo.Conditions");
            DropTable("dbo.Companies");
            DropTable("dbo.RecruitingSites");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
