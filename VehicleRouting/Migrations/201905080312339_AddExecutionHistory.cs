namespace VehicleRouting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExecutionHistory : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.ExecutionDurations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AlgorithmID = c.Int(nullable: false),
                        PointsOfDeliveryCount = c.Int(nullable: false),
                        ExecutionCount = c.Int(nullable: false),
                        MeanExecutionTimeTicks = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            this.DropTable("dbo.ExecutionDurations");
        }
    }
}
