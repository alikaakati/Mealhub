namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedFollowingTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Followings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        restaurant = c.String(),
                        customer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Followings");
        }
    }
}
