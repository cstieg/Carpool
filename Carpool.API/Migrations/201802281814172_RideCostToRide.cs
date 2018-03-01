namespace Carpool.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RideCostToRide : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdentityUserId = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        IsDriver = c.Boolean(nullable: false),
                        IsRider = c.Boolean(nullable: false),
                        DefaultRideCostId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RideCosts", t => t.DefaultRideCostId)
                .Index(t => t.IdentityUserId, unique: true)
                .Index(t => t.DefaultRideCostId);
            
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
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorUserId = c.Int(nullable: false),
                        DriverUserId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        RideCostId = c.Int(nullable: false),
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
                .ForeignKey("dbo.RideCosts", t => t.RideCostId, cascadeDelete: true)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorUserId, cascadeDelete: false)
                .Index(t => t.CreatorUserId)
                .Index(t => t.DriverUserId)
                .Index(t => t.VehicleId)
                .Index(t => t.RideCostId);
            
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
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Rides", t => t.RideId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RideId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        ModelYearId = c.Int(nullable: false),
                        Condition = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleModelYears", t => t.ModelYearId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: false)
                .Index(t => t.OwnerId)
                .Index(t => t.ModelYearId);
            
            CreateTable(
                "dbo.VehicleModelYears",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleModels", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "dbo.VehicleModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MakeId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VehicleMakes", t => t.MakeId, cascadeDelete: true)
                .Index(t => t.MakeId);
            
            CreateTable(
                "dbo.VehicleMakes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Vehicles", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Rides", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.Rides", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "ModelYearId", "dbo.VehicleModelYears");
            DropForeignKey("dbo.VehicleModelYears", "ModelId", "dbo.VehicleModels");
            DropForeignKey("dbo.VehicleModels", "MakeId", "dbo.VehicleMakes");
            DropForeignKey("dbo.Riders", "RideId", "dbo.Rides");
            DropForeignKey("dbo.Riders", "UserId", "dbo.Users");
            DropForeignKey("dbo.RideLegs", "RideId", "dbo.Rides");
            DropForeignKey("dbo.RideLegs", "OriginId", "dbo.Locations");
            DropForeignKey("dbo.RideLegs", "DestinationId", "dbo.Locations");
            DropForeignKey("dbo.Rides", "RideCostId", "dbo.RideCosts");
            DropForeignKey("dbo.Rides", "DriverUserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Locations", "LatLng_Id", "dbo.LatLngs");
            DropForeignKey("dbo.Locations", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Users", "DefaultRideCostId", "dbo.RideCosts");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.VehicleModels", new[] { "MakeId" });
            DropIndex("dbo.VehicleModelYears", new[] { "ModelId" });
            DropIndex("dbo.Vehicles", new[] { "ModelYearId" });
            DropIndex("dbo.Vehicles", new[] { "OwnerId" });
            DropIndex("dbo.Riders", new[] { "RideId" });
            DropIndex("dbo.Riders", new[] { "UserId" });
            DropIndex("dbo.RideLegs", new[] { "DestinationId" });
            DropIndex("dbo.RideLegs", new[] { "OriginId" });
            DropIndex("dbo.RideLegs", new[] { "RideId" });
            DropIndex("dbo.Rides", new[] { "RideCostId" });
            DropIndex("dbo.Rides", new[] { "VehicleId" });
            DropIndex("dbo.Rides", new[] { "DriverUserId" });
            DropIndex("dbo.Rides", new[] { "CreatorUserId" });
            DropIndex("dbo.Locations", new[] { "LatLng_Id" });
            DropIndex("dbo.Locations", new[] { "Address_Id" });
            DropIndex("dbo.Locations", new[] { "OwnerId" });
            DropIndex("dbo.Users", new[] { "DefaultRideCostId" });
            DropIndex("dbo.Users", new[] { "IdentityUserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "User_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.VehicleMakes");
            DropTable("dbo.VehicleModels");
            DropTable("dbo.VehicleModelYears");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Riders");
            DropTable("dbo.RideLegs");
            DropTable("dbo.Rides");
            DropTable("dbo.LatLngs");
            DropTable("dbo.Addresses");
            DropTable("dbo.Locations");
            DropTable("dbo.RideCosts");
            DropTable("dbo.Users");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
