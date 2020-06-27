namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmailColumnToTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Email", c => c.String());
            AddColumn("dbo.Restaurants", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restaurants", "Email");
            DropColumn("dbo.Customers", "Email");
        }
    }
}
