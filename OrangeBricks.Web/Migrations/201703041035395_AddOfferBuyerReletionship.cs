namespace OrangeBricks.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOfferBuyerReletionship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "BuyerUserId", c => c.String(nullable: false, maxLength: 128));
            AddForeignKey("dbo.Offers", "BuyerUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "BuyerUserId", "dbo.AspNetUsers");
            DropColumn("dbo.Offers", "BuyerUserId");
        }
    }
}
