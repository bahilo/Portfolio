namespace DagoWebPortfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DisplayViewModels", name: "ProjectDetail_ID", newName: "ProjectDetailsViewModelID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "Education_ID", newName: "EducationViewModelID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "Experience_ID", newName: "ExperiencesViewModelID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "Picture_ID", newName: "PicturesViewModelID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "Skill_ID", newName: "SkillsViewModelID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_ProjectDetail_ID", newName: "IX_ProjectDetailsViewModelID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_Education_ID", newName: "IX_EducationViewModelID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_Experience_ID", newName: "IX_ExperiencesViewModelID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_Skill_ID", newName: "IX_SkillsViewModelID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_Picture_ID", newName: "IX_PicturesViewModelID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_PicturesViewModelID", newName: "IX_Picture_ID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_SkillsViewModelID", newName: "IX_Skill_ID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_ExperiencesViewModelID", newName: "IX_Experience_ID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_EducationViewModelID", newName: "IX_Education_ID");
            RenameIndex(table: "dbo.DisplayViewModels", name: "IX_ProjectDetailsViewModelID", newName: "IX_ProjectDetail_ID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "SkillsViewModelID", newName: "Skill_ID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "PicturesViewModelID", newName: "Picture_ID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "ExperiencesViewModelID", newName: "Experience_ID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "EducationViewModelID", newName: "Education_ID");
            RenameColumn(table: "dbo.DisplayViewModels", name: "ProjectDetailsViewModelID", newName: "ProjectDetail_ID");
        }
    }
}
