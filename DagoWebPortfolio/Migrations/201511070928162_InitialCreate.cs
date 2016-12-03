namespace DagoWebPortfolio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

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
                "dbo.SkillsCategoryViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ContactViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Company = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectDetailsViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Client = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DisplayViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        lang = c.String(),
                        Description = c.String(nullable: false),
                        Education_ID = c.Int(),
                        Picture_ID = c.Int(),
                        Experience_ID = c.Int(),
                        Skill_ID = c.Int(),
                        ProjectDetail_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EducationViewModels", t => t.Education_ID)
                .ForeignKey("dbo.PicturesViewModels", t => t.Picture_ID)
                .ForeignKey("dbo.ExperiencesViewModels", t => t.Experience_ID)
                .ForeignKey("dbo.SkillsViewModels", t => t.Skill_ID)
                .ForeignKey("dbo.ProjectDetailsViewModels", t => t.ProjectDetail_ID)
                .Index(t => t.Education_ID)
                .Index(t => t.Picture_ID)
                .Index(t => t.Experience_ID)
                .Index(t => t.Skill_ID)
                .Index(t => t.ProjectDetail_ID);
            
            CreateTable(
                "dbo.EducationViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SchoolName = c.String(nullable: false),
                        YearGraduate = c.DateTime(nullable: false),
                        NbYearsToGraduate = c.Int(nullable: false),
                        link = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PicturesViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        path = c.String(),
                        FileName = c.String(),
                        link = c.String(),
                        IsAbout = c.Boolean(nullable: false),
                        IsWelcome = c.Boolean(nullable: false),
                        ProjectDetailsViewModelID = c.Int(),
                        EducationViewModelID = c.Int(),
                        ExperiencesViewModelID = c.Int(),
                        SkillsViewModelID = c.Int(),
                        EducationModel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EducationViewModels", t => t.EducationViewModelID)
                .ForeignKey("dbo.ExperiencesViewModels", t => t.ExperiencesViewModelID)
                .ForeignKey("dbo.SkillsViewModels", t => t.SkillsViewModelID)
                .ForeignKey("dbo.ProjectDetailsViewModels", t => t.ProjectDetailsViewModelID)
                .ForeignKey("dbo.EducationModels", t => t.EducationModel_ID)
                .Index(t => t.ProjectDetailsViewModelID)
                .Index(t => t.EducationViewModelID)
                .Index(t => t.ExperiencesViewModelID)
                .Index(t => t.SkillsViewModelID)
                .Index(t => t.EducationModel_ID);
            
            CreateTable(
                "dbo.ExperiencesViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Company = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        link = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SkillsViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        CategoryViewModel_ID = c.Int(),
                        LevelsViewModel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SkillsCategoryViewModels", t => t.CategoryViewModel_ID)
                .ForeignKey("dbo.SkillsLevelsViewModels", t => t.LevelsViewModel_ID)
                .Index(t => t.CategoryViewModel_ID)
                .Index(t => t.LevelsViewModel_ID);
            
            CreateTable(
                "dbo.SkillsLevelsViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        comments = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectsViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        link = c.String(),
                        Resume = c.String(),
                        ProjectDetail_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProjectDetailsViewModels", t => t.ProjectDetail_ID)
                .Index(t => t.ProjectDetail_ID);
            
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
            
            CreateTable(
                "dbo.SkillsViewModelExperiencesViewModels",
                c => new
                    {
                        SkillsViewModel_ID = c.Int(nullable: false),
                        ExperiencesViewModel_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SkillsViewModel_ID, t.ExperiencesViewModel_ID })
                .ForeignKey("dbo.SkillsViewModels", t => t.SkillsViewModel_ID, cascadeDelete: true)
                .ForeignKey("dbo.ExperiencesViewModels", t => t.ExperiencesViewModel_ID, cascadeDelete: true)
                .Index(t => t.SkillsViewModel_ID)
                .Index(t => t.ExperiencesViewModel_ID);
            
            CreateTable(
                "dbo.ProjectsViewModelSkillsViewModels",
                c => new
                    {
                        ProjectsViewModel_ID = c.Int(nullable: false),
                        SkillsViewModel_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProjectsViewModel_ID, t.SkillsViewModel_ID })
                .ForeignKey("dbo.ProjectsViewModels", t => t.ProjectsViewModel_ID, cascadeDelete: true)
                .ForeignKey("dbo.SkillsViewModels", t => t.SkillsViewModel_ID, cascadeDelete: true)
                .Index(t => t.ProjectsViewModel_ID)
                .Index(t => t.SkillsViewModel_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PicturesViewModels", "EducationModel_ID", "dbo.EducationModels");
            DropForeignKey("dbo.DisplayViewModels", "ProjectDetail_ID", "dbo.ProjectDetailsViewModels");
            DropForeignKey("dbo.PicturesViewModels", "ProjectDetailsViewModelID", "dbo.ProjectDetailsViewModels");
            DropForeignKey("dbo.ProjectsViewModelSkillsViewModels", "SkillsViewModel_ID", "dbo.SkillsViewModels");
            DropForeignKey("dbo.ProjectsViewModelSkillsViewModels", "ProjectsViewModel_ID", "dbo.ProjectsViewModels");
            DropForeignKey("dbo.ProjectsViewModels", "ProjectDetail_ID", "dbo.ProjectDetailsViewModels");
            DropForeignKey("dbo.PicturesViewModels", "SkillsViewModelID", "dbo.SkillsViewModels");
            DropForeignKey("dbo.SkillsViewModels", "LevelsViewModel_ID", "dbo.SkillsLevelsViewModels");
            DropForeignKey("dbo.SkillsViewModelExperiencesViewModels", "ExperiencesViewModel_ID", "dbo.ExperiencesViewModels");
            DropForeignKey("dbo.SkillsViewModelExperiencesViewModels", "SkillsViewModel_ID", "dbo.SkillsViewModels");
            DropForeignKey("dbo.DisplayViewModels", "Skill_ID", "dbo.SkillsViewModels");
            DropForeignKey("dbo.SkillsViewModels", "CategoryViewModel_ID", "dbo.SkillsCategoryViewModels");
            DropForeignKey("dbo.PicturesViewModels", "ExperiencesViewModelID", "dbo.ExperiencesViewModels");
            DropForeignKey("dbo.DisplayViewModels", "Experience_ID", "dbo.ExperiencesViewModels");
            DropForeignKey("dbo.PicturesViewModels", "EducationViewModelID", "dbo.EducationViewModels");
            DropForeignKey("dbo.DisplayViewModels", "Picture_ID", "dbo.PicturesViewModels");
            DropForeignKey("dbo.DisplayViewModels", "Education_ID", "dbo.EducationViewModels");
            DropIndex("dbo.ProjectsViewModelSkillsViewModels", new[] { "SkillsViewModel_ID" });
            DropIndex("dbo.ProjectsViewModelSkillsViewModels", new[] { "ProjectsViewModel_ID" });
            DropIndex("dbo.SkillsViewModelExperiencesViewModels", new[] { "ExperiencesViewModel_ID" });
            DropIndex("dbo.SkillsViewModelExperiencesViewModels", new[] { "SkillsViewModel_ID" });
            DropIndex("dbo.ProjectsViewModels", new[] { "ProjectDetail_ID" });
            DropIndex("dbo.SkillsViewModels", new[] { "LevelsViewModel_ID" });
            DropIndex("dbo.SkillsViewModels", new[] { "CategoryViewModel_ID" });
            DropIndex("dbo.PicturesViewModels", new[] { "EducationModel_ID" });
            DropIndex("dbo.PicturesViewModels", new[] { "SkillsViewModelID" });
            DropIndex("dbo.PicturesViewModels", new[] { "ExperiencesViewModelID" });
            DropIndex("dbo.PicturesViewModels", new[] { "EducationViewModelID" });
            DropIndex("dbo.PicturesViewModels", new[] { "ProjectDetailsViewModelID" });
            DropIndex("dbo.DisplayViewModels", new[] { "ProjectDetail_ID" });
            DropIndex("dbo.DisplayViewModels", new[] { "Skill_ID" });
            DropIndex("dbo.DisplayViewModels", new[] { "Experience_ID" });
            DropIndex("dbo.DisplayViewModels", new[] { "Picture_ID" });
            DropIndex("dbo.DisplayViewModels", new[] { "Education_ID" });
            DropTable("dbo.ProjectsViewModelSkillsViewModels");
            DropTable("dbo.SkillsViewModelExperiencesViewModels");
            DropTable("dbo.EducationModels");
            DropTable("dbo.ProjectsViewModels");
            DropTable("dbo.SkillsLevelsViewModels");
            DropTable("dbo.SkillsViewModels");
            DropTable("dbo.ExperiencesViewModels");
            DropTable("dbo.PicturesViewModels");
            DropTable("dbo.EducationViewModels");
            DropTable("dbo.DisplayViewModels");
            DropTable("dbo.ProjectDetailsViewModels");
            DropTable("dbo.ContactViewModels");
            DropTable("dbo.SkillsCategoryViewModels");

            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
        }
    }
}
