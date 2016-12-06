
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
        public DbSet<DisplayViewModel> Displays { get; set; }
        public DbSet<TechnoEnvironmentsViewModel> TechnoEnv { get; set; }

    } 

    public class MockInitializer : DropCreateDatabaseIfModelChanges<DBModelPortfolioContext>
        //public class MockInitializer : DropCreateDatabaseAlways<DBModelPortfolioContext>
    {
        protected override void Seed(DBModelPortfolioContext context)
        {
            base.Seed(context);
            
            //EducationViewModel education = new EducationViewModel { SchoolName="test", NbYearsToGraduate=2};
            //context.Education.Add(education);
            //context.SaveChanges();
        }
    }
}
