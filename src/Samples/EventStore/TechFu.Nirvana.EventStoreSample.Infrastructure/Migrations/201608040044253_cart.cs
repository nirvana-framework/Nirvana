namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SiteUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsAnonomous = c.Boolean(nullable: false),
                        LastLogin = c.DateTime(nullable: false),
                        LoginCount = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Carts", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Carts", "UserId");
            DropTable("dbo.SiteUsers");
        }
    }
}
