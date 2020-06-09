namespace MealHubProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedTheRegistrationForCustomer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Username", c => c.String(nullable: false));
            DropColumn("dbo.Customers", "Password");
            DropColumn("dbo.Customers", "Email");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "Email", c => c.String());
            AddColumn("dbo.Customers", "Password", c => c.String());
            AlterColumn("dbo.Customers", "Username", c => c.String());
        }
    }
}
