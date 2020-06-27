namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedAnInBetweenTableThatIncludesProductsAndOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductsInOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProductsInOrders");
        }
    }
}
