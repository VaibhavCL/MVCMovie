namespace MvcMovie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Movie : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "Price", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
