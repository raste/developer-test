namespace OrangeBricks.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppointmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BuyerUserId = c.String(nullable: false, maxLength: 128),
                        Status = c.Int(nullable: false),
                        Property_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Properties", t => t.Property_Id)
                .Index(t => t.Property_Id);

            AddForeignKey("dbo.Appointments", "BuyerUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "BuyerUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "Property_Id", "dbo.Properties");
            DropIndex("dbo.Appointments", new[] { "Property_Id" });
            DropTable("dbo.Appointments");
        }
    }
}
