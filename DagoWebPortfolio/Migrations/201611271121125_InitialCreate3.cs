namespace DagoWebPortfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DisplayViewModels", "ProjectsViewModelID", c => c.Int());
            CreateIndex("dbo.DisplayViewModels", "ProjectsViewModelID");
            AddForeignKey("dbo.DisplayViewModels", "ProjectsViewModelID", "dbo.ProjectsViewModels", "ID");
            DropColumn("dbo.ProjectsViewModels", "Resume");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectsViewModels", "Resume", c => c.String());
            DropForeignKey("dbo.DisplayViewModels", "ProjectsViewModelID", "dbo.ProjectsViewModels");
            DropIndex("dbo.DisplayViewModels", new[] { "ProjectsViewModelID" });
            DropColumn("dbo.DisplayViewModels", "ProjectsViewModelID");
        }
    }
}
