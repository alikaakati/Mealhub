namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedRestaurantsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Restaurants", "ApplicationUserId");
            AddForeignKey("dbo.Restaurants", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Restaurants", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Restaurants", new[] { "ApplicationUserId" });
            DropColumn("dbo.Restaurants", "ApplicationUserId");
        }
    }
}
