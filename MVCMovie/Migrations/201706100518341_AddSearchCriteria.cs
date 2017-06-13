namespace MVCMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchCriteria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SearchCriterias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RecruitingSiteId = c.Int(nullable: false),
                        FieldName = c.String(),
                        CriteriaOperator = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RecruitingSites", t => t.RecruitingSiteId, cascadeDelete: true)
                .Index(t => t.RecruitingSiteId);
            
            CreateTable(
                "dbo.SearchCriteriaValues",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SearchCriteriaId = c.Int(nullable: false),
                        value = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SearchCriterias", t => t.SearchCriteriaId, cascadeDelete: true)
                .Index(t => t.SearchCriteriaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SearchCriteriaValues", "SearchCriteriaId", "dbo.SearchCriterias");
            DropForeignKey("dbo.SearchCriterias", "RecruitingSiteId", "dbo.RecruitingSites");
            DropIndex("dbo.SearchCriteriaValues", new[] { "SearchCriteriaId" });
            DropIndex("dbo.SearchCriterias", new[] { "RecruitingSiteId" });
            DropTable("dbo.SearchCriteriaValues");
            DropTable("dbo.SearchCriterias");
        }
    }
}
