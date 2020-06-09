namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedRestaurantsTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Restaurants", "Username", c => c.String(nullable: false));
            DropColumn("dbo.Restaurants", "Password");
            DropColumn("dbo.Restaurants", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Restaurants", "Email", c => c.String());
            AddColumn("dbo.Restaurants", "Password", c => c.String());
            AlterColumn("dbo.Restaurants", "Username", c => c.String());
        }
    }
}
