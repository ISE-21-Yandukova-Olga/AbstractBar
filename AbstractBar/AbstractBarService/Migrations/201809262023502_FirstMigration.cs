namespace AbstractBarService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Barmen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BarmenFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        CoctailId = c.Int(nullable: false),
                        BarmenId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateImplement = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Barmen", t => t.BarmenId)
                .ForeignKey("dbo.Coctails", t => t.CoctailId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.CoctailId)
                .Index(t => t.BarmenId);
            
            CreateTable(
                "dbo.Coctails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoctailName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CoctailIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoctailId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coctails", t => t.CoctailId, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .Index(t => t.CoctailId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngredientName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StorageIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Storages", t => t.StorageId, cascadeDelete: true)
                .Index(t => t.StorageId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Requests", "CoctailId", "dbo.Coctails");
            DropForeignKey("dbo.StorageIngredients", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.StorageIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.CoctailIngredients", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.CoctailIngredients", "CoctailId", "dbo.Coctails");
            DropForeignKey("dbo.Requests", "BarmenId", "dbo.Barmen");
            DropIndex("dbo.StorageIngredients", new[] { "IngredientId" });
            DropIndex("dbo.StorageIngredients", new[] { "StorageId" });
            DropIndex("dbo.CoctailIngredients", new[] { "IngredientId" });
            DropIndex("dbo.CoctailIngredients", new[] { "CoctailId" });
            DropIndex("dbo.Requests", new[] { "BarmenId" });
            DropIndex("dbo.Requests", new[] { "CoctailId" });
            DropIndex("dbo.Requests", new[] { "CustomerId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Storages");
            DropTable("dbo.StorageIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.CoctailIngredients");
            DropTable("dbo.Coctails");
            DropTable("dbo.Requests");
            DropTable("dbo.Barmen");
        }
    }
}
