namespace DagoWebPortfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PicturesViewModels", "EducationModel_ID", "dbo.EducationModels");
            DropIndex("dbo.PicturesViewModels", new[] { "EducationModel_ID" });
            CreateTable(
                "dbo.TechnoEnvironmentsViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Category = c.String(),
                        path = c.String(),
                        FileName = c.String(),
                        link = c.String(),
                        ProjectsViewModelID = c.Int(),
                        ProjectDetailsViewModelID = c.Int(),
                        EducationViewModelID = c.Int(),
                        ExperiencesViewModelID = c.Int(),
                        SkillsViewModelID = c.Int(),
                        PicturesViewModelID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EducationViewModels", t => t.EducationViewModelID)
                .ForeignKey("dbo.ExperiencesViewModels", t => t.ExperiencesViewModelID)
                .ForeignKey("dbo.PicturesViewModels", t => t.PicturesViewModelID)
                .ForeignKey("dbo.ProjectsViewModels", t => t.ProjectsViewModelID)
                .ForeignKey("dbo.ProjectDetailsViewModels", t => t.ProjectDetailsViewModelID)
                .ForeignKey("dbo.SkillsViewModels", t => t.SkillsViewModelID)
                .Index(t => t.ProjectsViewModelID)
                .Index(t => t.ProjectDetailsViewModelID)
                .Index(t => t.EducationViewModelID)
                .Index(t => t.ExperiencesViewModelID)
                .Index(t => t.SkillsViewModelID)
                .Index(t => t.PicturesViewModelID);
            
            AddColumn("dbo.DisplayViewModels", "TechEnvsViewModelID", c => c.Int());
            CreateIndex("dbo.DisplayViewModels", "TechEnvsViewModelID");
            AddForeignKey("dbo.DisplayViewModels", "TechEnvsViewModelID", "dbo.TechnoEnvironmentsViewModels", "ID");
            DropColumn("dbo.PicturesViewModels", "EducationModel_ID");
            DropTable("dbo.EducationModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EducationModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SchoolName = c.String(nullable: false),
                        YearGraduate = c.DateTime(nullable: false),
                        NbYearsToGraduate = c.Int(nullable: false),
                        link = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.PicturesViewModels", "EducationModel_ID", c => c.Int());
            DropForeignKey("dbo.TechnoEnvironmentsViewModels", "SkillsViewModelID", "dbo.SkillsViewModels");
            DropForeignKey("dbo.TechnoEnvironmentsViewModels", "ProjectDetailsViewModelID", "dbo.ProjectDetailsViewModels");
            DropForeignKey("dbo.TechnoEnvironmentsViewModels", "ProjectsViewModelID", "dbo.ProjectsViewModels");
            DropForeignKey("dbo.TechnoEnvironmentsViewModels", "PicturesViewModelID", "dbo.PicturesViewModels");
            DropForeignKey("dbo.TechnoEnvironmentsViewModels", "ExperiencesViewModelID", "dbo.ExperiencesViewModels");
            DropForeignKey("dbo.TechnoEnvironmentsViewModels", "EducationViewModelID", "dbo.EducationViewModels");
            DropForeignKey("dbo.DisplayViewModels", "TechEnvsViewModelID", "dbo.TechnoEnvironmentsViewModels");
            DropIndex("dbo.TechnoEnvironmentsViewModels", new[] { "PicturesViewModelID" });
            DropIndex("dbo.TechnoEnvironmentsViewModels", new[] { "SkillsViewModelID" });
            DropIndex("dbo.TechnoEnvironmentsViewModels", new[] { "ExperiencesViewModelID" });
            DropIndex("dbo.TechnoEnvironmentsViewModels", new[] { "EducationViewModelID" });
            DropIndex("dbo.TechnoEnvironmentsViewModels", new[] { "ProjectDetailsViewModelID" });
            DropIndex("dbo.TechnoEnvironmentsViewModels", new[] { "ProjectsViewModelID" });
            DropIndex("dbo.DisplayViewModels", new[] { "TechEnvsViewModelID" });
            DropColumn("dbo.DisplayViewModels", "TechEnvsViewModelID");
            DropTable("dbo.TechnoEnvironmentsViewModels");
            CreateIndex("dbo.PicturesViewModels", "EducationModel_ID");
            AddForeignKey("dbo.PicturesViewModels", "EducationModel_ID", "dbo.EducationModels", "ID");
        }
    }
}
