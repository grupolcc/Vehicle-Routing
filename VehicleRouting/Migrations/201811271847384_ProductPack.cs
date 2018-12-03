namespace VehicleRouting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class ProductPack : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.ProductPacks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        PointOfDelivery_ID = c.Int(nullable: false),
                        Product_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PointOfDeliveries", t => t.PointOfDelivery_ID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .Index(t => t.PointOfDelivery_ID)
                .Index(t => t.Product_ID);
            
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.ProductPacks", "Product_ID", "dbo.Products");
            this.DropForeignKey("dbo.ProductPacks", "PointOfDelivery_ID", "dbo.PointOfDeliveries");
            this.DropIndex("dbo.ProductPacks", new[] { "Product_ID" });
            this.DropIndex("dbo.ProductPacks", new[] { "PointOfDelivery_ID" });
            this.DropTable("dbo.ProductPacks");
        }
    }
}
