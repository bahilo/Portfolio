namespace DagoWebPortfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisplayViewModels", "Subject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DisplayViewModels", "Subject");
        }
    }
}
