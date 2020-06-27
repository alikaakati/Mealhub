namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedSomeErrorsInDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductsToOrders", "CustomerID", c => c.Int(nullable: false));
            DropColumn("dbo.ProductsToOrders", "CustomeriID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductsToOrders", "CustomeriID", c => c.Int(nullable: false));
            DropColumn("dbo.ProductsToOrders", "CustomerID");
        }
    }
}
