namespace OrangeBricks.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfferBuyerReletionship : DbMigration
    {
        public override void Up()
        {
            ///nullable: true so that the migration can work with previously created databases
            ///By default it shouldn't be however
            AddColumn("dbo.Offers", "BuyerUserId", c => c.String(nullable: true, maxLength: 128));
            AddForeignKey("dbo.Offers", "BuyerUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "BuyerUserId", "dbo.AspNetUsers");
            DropColumn("dbo.Offers", "BuyerUserId");
        }
    }
}
