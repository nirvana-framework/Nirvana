namespace TechFu.Nirvana.EventStoreSample.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameToCart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartItems", "Name");
        }
    }
}
