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
                        Coctail_Id = c.Int(nullable: false),
                        BarmenId = c.Int(),
                        Count = c.Int(nullable: false),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateBarmen = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Barmen", t => t.BarmenId)
                .ForeignKey("dbo.Coctails", t => t.Coctail_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.Coctail_Id)
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
                "dbo.IngredientsCoctails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Coctail_Id = c.Int(nullable: false),
                        Ingredients_Id = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coctails", t => t.Coctail_Id, cascadeDelete: true)
                .ForeignKey("dbo.Ingredients", t => t.Ingredients_Id, cascadeDelete: true)
                .Index(t => t.Coctail_Id)
                .Index(t => t.Ingredients_Id);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngredientsName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StorageIngredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageId = c.Int(nullable: false),
                        Ingredients_Id = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.Ingredients_Id, cascadeDelete: true)
                .ForeignKey("dbo.Storages", t => t.StorageId, cascadeDelete: true)
                .Index(t => t.StorageId)
                .Index(t => t.Ingredients_Id);
            
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
            DropForeignKey("dbo.Requests", "Coctail_Id", "dbo.Coctails");
            DropForeignKey("dbo.StorageIngredients", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.StorageIngredients", "Ingredients_Id", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientsCoctails", "Ingredients_Id", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientsCoctails", "Coctail_Id", "dbo.Coctails");
            DropForeignKey("dbo.Requests", "BarmenId", "dbo.Barmen");
            DropIndex("dbo.StorageIngredients", new[] { "Ingredients_Id" });
            DropIndex("dbo.StorageIngredients", new[] { "StorageId" });
            DropIndex("dbo.IngredientsCoctails", new[] { "Ingredients_Id" });
            DropIndex("dbo.IngredientsCoctails", new[] { "Coctail_Id" });
            DropIndex("dbo.Requests", new[] { "BarmenId" });
            DropIndex("dbo.Requests", new[] { "Coctail_Id" });
            DropIndex("dbo.Requests", new[] { "CustomerId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Storages");
            DropTable("dbo.StorageIngredients");
            DropTable("dbo.Ingredients");
            DropTable("dbo.IngredientsCoctails");
            DropTable("dbo.Coctails");
            DropTable("dbo.Requests");
            DropTable("dbo.Barmen");
        }
    }
}
