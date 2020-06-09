namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedOrdersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Order_Id", c => c.Int());
            CreateIndex("dbo.Products", "Order_Id");
            AddForeignKey("dbo.Products", "Order_Id", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Order_Id", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "Order_Id" });
            DropColumn("dbo.Products", "Order_Id");
        }
    }
}
