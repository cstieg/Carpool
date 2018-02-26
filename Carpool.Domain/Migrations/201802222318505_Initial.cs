namespace Carpool.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vehicles", "Model_Id", "dbo.VehicleModels");
            DropIndex("dbo.Vehicles", new[] { "Model_Id" });
            RenameColumn(table: "dbo.VehicleModels", name: "Make_Id", newName: "MakeId");
            RenameColumn(table: "dbo.Vehicles", name: "Model_Id", newName: "ModelId");
            RenameColumn(table: "dbo.Vehicles", name: "Owner_Id", newName: "OwnerId");
            RenameIndex(table: "dbo.Vehicles", name: "IX_Owner_Id", newName: "IX_OwnerId");
            RenameIndex(table: "dbo.VehicleModels", name: "IX_Make_Id", newName: "IX_MakeId");
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        City = c.String(),
                        State = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LatLngs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Address_Id = c.Int(),
                        LatLng_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.LatLngs", t => t.LatLng_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.Address_Id)
                .Index(t => t.LatLng_Id);
            
            CreateTable(
                "dbo.RideCosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BaseCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostPerMile = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PickupCostPerMile = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PickupCostPerHour = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PickupDistanceMileLimit = c.Int(nullable: false),
                        PickupTimeLimitMinutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorUserId = c.Int(nullable: false),
                        DriverUserId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        StartDateTime = c.DateTime(nullable: false),
                        ReturnDateTime = c.DateTime(nullable: false),
                        Repeating = c.Byte(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        AvailableSeats = c.Int(nullable: false),
                        IsSeekingRiders = c.Boolean(nullable: false),
                        IsSeekingDriver = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DriverUserId, cascadeDelete: false)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorUserId, cascadeDelete: false)
                .Index(t => t.CreatorUserId)
                .Index(t => t.DriverUserId)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.RideLegs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RideId = c.Int(nullable: false),
                        OriginId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                        Distance = c.Single(nullable: false),
                        Time = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.DestinationId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.OriginId, cascadeDelete: false)
                .ForeignKey("dbo.Rides", t => t.RideId, cascadeDelete: true)
                .Index(t => t.RideId)
                .Index(t => t.OriginId)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Riders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RideId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Rides", t => t.RideId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RideId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        RevieweeId = c.Int(nullable: false),
                        RevieweeWasDriver = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Stars = c.Int(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.RevieweeId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RevieweeId);
            
            AddColumn("dbo.Users", "IdentityUserId", c => c.Guid(nullable: false));
            AddColumn("dbo.Users", "IsDriver", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "IsRider", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "DefaultRideCostId", c => c.Int());
            AddColumn("dbo.Vehicles", "Condition", c => c.String());
            AlterColumn("dbo.Vehicles", "ModelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "IdentityUserId", unique: true);
            CreateIndex("dbo.Users", "DefaultRideCostId");
            CreateIndex("dbo.Vehicles", "ModelId");
            AddForeignKey("dbo.Users", "DefaultRideCostId", "dbo.RideCosts", "Id");
            AddForeignKey("dbo.Vehicles", "ModelId", "dbo.VehicleModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "ModelId", "dbo.VehicleModels");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "RevieweeId", "dbo.Users");
            DropForeignKey("dbo.Rides", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Rides", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Riders", "RideId", "dbo.Rides");
            DropForeignKey("dbo.Riders", "UserId", "dbo.Users");
            DropForeignKey("dbo.RideLegs", "RideId", "dbo.Rides");
            DropForeignKey("dbo.RideLegs", "OriginId", "dbo.Locations");
            DropForeignKey("dbo.RideLegs", "DestinationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "DriverUserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Users", "DefaultRideCostId", "dbo.RideCosts");
            DropForeignKey("dbo.Locations", "LatLng_Id", "dbo.LatLngs");
            DropForeignKey("dbo.Locations", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Reviews", new[] { "RevieweeId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Vehicles", new[] { "ModelId" });
            DropIndex("dbo.Riders", new[] { "RideId" });
            DropIndex("dbo.Riders", new[] { "UserId" });
            DropIndex("dbo.RideLegs", new[] { "DestinationId" });
            DropIndex("dbo.RideLegs", new[] { "OriginId" });
            DropIndex("dbo.RideLegs", new[] { "RideId" });
            DropIndex("dbo.Rides", new[] { "VehicleId" });
            DropIndex("dbo.Rides", new[] { "DriverUserId" });
            DropIndex("dbo.Rides", new[] { "CreatorUserId" });
            DropIndex("dbo.Users", new[] { "DefaultRideCostId" });
            DropIndex("dbo.Users", new[] { "IdentityUserId" });
            DropIndex("dbo.Locations", new[] { "LatLng_Id" });
            DropIndex("dbo.Locations", new[] { "Address_Id" });
            DropIndex("dbo.Locations", new[] { "OwnerId" });
            AlterColumn("dbo.Vehicles", "ModelId", c => c.Int());
            DropColumn("dbo.Vehicles", "Condition");
            DropColumn("dbo.Users", "DefaultRideCostId");
            DropColumn("dbo.Users", "IsRider");
            DropColumn("dbo.Users", "IsDriver");
            DropColumn("dbo.Users", "IdentityUserId");
            DropTable("dbo.Reviews");
            DropTable("dbo.Riders");
            DropTable("dbo.RideLegs");
            DropTable("dbo.Rides");
            DropTable("dbo.RideCosts");
            DropTable("dbo.Locations");
            DropTable("dbo.LatLngs");
            DropTable("dbo.Addresses");
            RenameIndex(table: "dbo.VehicleModels", name: "IX_MakeId", newName: "IX_Make_Id");
            RenameIndex(table: "dbo.Vehicles", name: "IX_OwnerId", newName: "IX_Owner_Id");
            RenameColumn(table: "dbo.Vehicles", name: "OwnerId", newName: "Owner_Id");
            RenameColumn(table: "dbo.Vehicles", name: "ModelId", newName: "Model_Id");
            RenameColumn(table: "dbo.VehicleModels", name: "MakeId", newName: "Make_Id");
            CreateIndex("dbo.Vehicles", "Model_Id");
            AddForeignKey("dbo.Vehicles", "Model_Id", "dbo.VehicleModels", "Id");
        }
    }
}
