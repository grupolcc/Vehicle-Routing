namespace VehicleRouting.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class foreignkeys : DbMigration
    {
        public override void Up()
        {
            this.RenameColumn(table: "dbo.ProductPacks", name: "PointOfDelivery_ID", newName: "PointOfDeliveryID");
            this.RenameColumn(table: "dbo.ProductPacks", name: "Product_ID", newName: "ProductID");
            this.RenameIndex(table: "dbo.ProductPacks", name: "IX_Product_ID", newName: "IX_ProductID");
            this.RenameIndex(table: "dbo.ProductPacks", name: "IX_PointOfDelivery_ID", newName: "IX_PointOfDeliveryID");
        }
        
        public override void Down()
        {
            this.RenameIndex(table: "dbo.ProductPacks", name: "IX_PointOfDeliveryID", newName: "IX_PointOfDelivery_ID");
            this.RenameIndex(table: "dbo.ProductPacks", name: "IX_ProductID", newName: "IX_Product_ID");
            this.RenameColumn(table: "dbo.ProductPacks", name: "ProductID", newName: "Product_ID");
            this.RenameColumn(table: "dbo.ProductPacks", name: "PointOfDeliveryID", newName: "PointOfDelivery_ID");
        }
    }
}
