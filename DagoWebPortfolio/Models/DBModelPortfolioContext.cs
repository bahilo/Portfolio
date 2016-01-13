
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DagoWebPortfolio.Models
{
    public class DBModelPortfolioContext : DbContext
    {
        public DbSet<ExperiencesViewModel> Experiences { get; set; }
        public DbSet<ProjectsViewModel> Projects { get; set; }
        public DbSet<SkillsViewModel> Skills { get; set; }
        public DbSet<EducationViewModel> Education { get; set; }
        public DbSet<SkillsLevelsViewModel> Level { get; set; }
        public DbSet<SkillsCategoryViewModel> Categories { get; set; }
        public DbSet<ProjectDetailsViewModel> DetailsProject { get; set; }
        public DbSet<PicturesViewModel> PicturesApp { get; set; }
        public DbSet<ContactViewModel> Contacts { get; set; }

    } 

    public class MockInitializer : DropCreateDatabaseAlways<DBModelPortfolioContext>
    {
        protected override void Seed(DBModelPortfolioContext context)
        {
            base.Seed(context);

            var Skill1 = new SkillsViewModel { ID = 1, Title = "PHP", Description = "Very good skill" };
            var Category1 = new SkillsCategoryViewModel { ID = 1, Title = "Coding", Description = "Category[Coding] -> Very good skill" };
            var Level1 = new SkillsLevelsViewModel { ID = 1, Level = 85, comments = "Level[Coding] -> Very good skill" };
            var Project1 = new ProjectsViewModel { ID = 1, Title = "Bird Watching", Resume = "Bird Watching" };
            var DetailProject1 = new ProjectDetailsViewModel { ID = 1, Subject = "Bird Watching", Status = false, Client = "Bahilo sisi 1", Date = DateTime.Now, Description = "Description eetail project3" };

            var Skill2 = new SkillsViewModel { ID = 2, Title = "Client Satisfactions", Description = "Very good skill" };
            var Category2 = new SkillsCategoryViewModel { ID = 2, Title = "Business", Description = "Category[Business - Client Satisfactions] -> Very good skill" };
            var Level2 = new SkillsLevelsViewModel { ID = 2, Level = 85, comments = "Level[Business] -> Very good skill" };
            var Project2 = new ProjectsViewModel { ID = 2, Title = "Basket weaving", Resume = "Basket weaving for beginners" };
            var DetailProject2 = new ProjectDetailsViewModel { ID = 2, Subject= "Basket weaving", Status = false, Client = "Bahilo sisi 2", Date = DateTime.Now, Description = "Description eetail project2" };

            var Skill3 = new SkillsViewModel { ID = 3, Title = "Pressure handling", Description = "Very good skill" };
            var Category3 = new SkillsCategoryViewModel { ID = 3, Title = "Others", Description = "Category[Others] -> Very good skill" };
            var Level3 = new SkillsLevelsViewModel { ID = 3, Level = 85, comments = "Level[Others] -> Very good skill" };
            var Project3 = new ProjectsViewModel { ID = 3, Title = "Photography", Resume = "Photography 101" };
            var DetailProject3 = new ProjectDetailsViewModel { ID = 3, Subject = "Photography", Status = true, Client = "Bahilo sisi 3", Date = DateTime.Now, Description = "Description eetail project3" };

            var Skill4 = new SkillsViewModel { ID = 4, Title = "ASP.NET", Description = "Very good skill" };
            var Category4 = new SkillsCategoryViewModel { ID = 4, Title = "Coding", Description = "Category[Coding] -> Very good skill" };
            var Level4 = new SkillsLevelsViewModel { ID = 4, Level = 65, comments = "Level[Coding] -> Very good skill" };
            var Experience1 = new ExperiencesViewModel { ID = 1, Title = "Engineer Developper", Company = "Infoelsa", Responsabilities = "Developing solutions", StartDate = new DateTime(2014, 12, 22), EndDate = new DateTime(2015, 4, 10) };

            var Skill5 = new SkillsViewModel { ID = 5, Title = "Communication", Description = "Very good skill" };
            var Category5 = new SkillsCategoryViewModel { ID = 5, Title = "Business", Description = "Category[Business - Communication] -> Very good skill" };
            var Level5 = new SkillsLevelsViewModel { ID = 5, Level = 75, comments = "Level[Business] -> Very good skill" };
            var Experience2 = new ExperiencesViewModel { ID = 2, Title = "Apprentice Engineer", Company = "Orange", Responsabilities = "Maintaining IT environnement", StartDate = new DateTime(2011, 11, 2), EndDate = new DateTime(2014, 10, 30) };
            

            var Education = new EducationViewModel { ID = 1, NbYearsToGraduate = 3, SchoolName = "ISEP", YearGraduate = new DateTime(2014,10,30)};
            var Education2 = new EducationViewModel { ID = 2, NbYearsToGraduate = 3, SchoolName = "IUT Ville d'AVRAY", YearGraduate = new DateTime(2014, 10, 30) };


            var picture1 = new PicturesViewModel { ID = 1, Subject = "Experience", Description = "Experience Picture description", FileName = "f4769_paris night.jpg", path = "/Content/Images/Experiences/" };
            var picture2 = new PicturesViewModel { ID = 2, Subject = "Project", Description = "Project Picture description", FileName = "f4769_paris night.jpg", path = "/Content/Images/Experiences/" };
            var picture3 = new PicturesViewModel { ID = 3, Subject = "Project", Description = "Project Picture description", FileName = "f4769_paris night.jpg", path = "/Content/Images/Experiences/" };
            var picture4 = new PicturesViewModel { ID = 4, Subject = "Project", Description = "Project Picture description", FileName = "f4769_paris night.jpg", path = "/Content/Images/Experiences/" };

            picture2.ProjectDetail = DetailProject1;
            picture2.ProjectDetailsViewModelID = DetailProject1.ID;
            picture2.Experience = null;
            picture2.ExperiencesViewModelID = null;
            picture2.Skill = null;
            picture2.SkillsViewModelID = null;
            picture2.Education = null;
            picture2.EducationViewModelID = null;

            DetailProject1.Pictures.Add(picture2);
            Project1.ProjectDetail = DetailProject1;
            Skill1.CategoryViewModel = Category1;
            Skill1.LevelsViewModel = Level1;
            Skill1.Projects.Add( Project1 );

            Project2.ProjectDetail = DetailProject2;
            Skill2.CategoryViewModel = Category2;
            Skill2.LevelsViewModel = Level2;
            Skill2.Projects.Add( Project2 );

            Project3.ProjectDetail = DetailProject3;
            Skill3.CategoryViewModel = Category3;
            Skill3.LevelsViewModel = Level3;
            Skill3.Projects.Add( Project3 );

            Skill4.CategoryViewModel = Category4;
            Skill4.LevelsViewModel = Level4;
            Skill4.Experiences.Add( Experience1 );

            picture1.Experience = Experience2;
            picture1.ExperiencesViewModelID = Experience2.ID;
            picture1.ProjectDetail = null;
            picture1.ProjectDetailsViewModelID = null;
            picture1.Skill = null;
            picture1.SkillsViewModelID = null;
            picture1.Education = null;
            picture1.EducationViewModelID = null;
            Experience2.Pictures.Add(picture1);
            Skill5.CategoryViewModel = Category5;
            Skill5.LevelsViewModel = Level5;
            Skill5.Experiences.Add(Experience2);

            context.Skills.Add(Skill1);
            context.Skills.Add(Skill2);
            context.Skills.Add(Skill3);
            context.Skills.Add(Skill4);
            context.Skills.Add(Skill5);

            picture3.Education = Education;
            picture3.EducationViewModelID = Education.ID;
            picture3.Experience = null;
            picture3.ExperiencesViewModelID = null;
            picture3.Skill = null;
            picture3.SkillsViewModelID = null;
            picture3.ProjectDetail = null;
            picture3.ProjectDetailsViewModelID = null;

            Education.Pictures.Add(picture3);

            picture4.Education = Education2;
            picture4.EducationViewModelID = Education2.ID;
            picture4.Experience = null;
            picture4.ExperiencesViewModelID = null;
            picture4.Skill = null;
            picture4.SkillsViewModelID = null;
            picture4.ProjectDetail = null;
            picture4.ProjectDetailsViewModelID = null;

            Education2.Pictures.Add(picture4);

            context.Education.Add(Education);
            context.Education.Add(Education2);
        }
    }
}
