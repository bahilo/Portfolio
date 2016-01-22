
using DagoWebPortfolio.Models.DisplayViewModel;
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

    } 

    public class MockInitializer : DropCreateDatabaseIfModelChanges<DBModelPortfolioContext>
    {
        protected override void Seed(DBModelPortfolioContext context)
        {
            base.Seed(context);

            
            //=========================================[ Skills ]=========================================================

            var skills = new List<SkillsViewModel>();

            var skill = new SkillsViewModel { ID = 1, Title = "PHP", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 1, Title = "Coding", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 1, Level = 85, comments = "To Do" };
            skills.Add(skill);

            skill = new SkillsViewModel { ID = 2, Title = "JAVA J2E", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 2, Title = "Coding", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 2, Level = 75, comments = "To Do" };
            skills.Add(skill);            

            skill = new SkillsViewModel { ID = 3, Title = "MySQL", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 3, Title = "Coding", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 3, Level = 80, comments = "To Do" };
            skills.Add(skill);

            skill = new SkillsViewModel { ID = 4, Title = "HTML / CSS", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 4, Title = "Coding", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 4, Level = 85, comments = "To Do" };
            skills.Add(skill);

            skill = new SkillsViewModel { ID = 5, Title = "French", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 5, Title = "Others", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 5, Level = 100, comments = "To Do" };
            skills.Add(skill);

            skill = new SkillsViewModel { ID = 6, Title = "Client Satisfactions", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 6, Title = "Business", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 6, Level = 85, comments = "To Do" };
            skills.Add(skill);

            skill = new SkillsViewModel { ID = 7, Title = "Pressure handling", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 7, Title = "Others", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 7, Level = 85, comments = "To Do" };
            skills.Add(skill);

            skill = new SkillsViewModel { ID = 8, Title = "Communication", Description = "Very good skill" };
            skill.CategoryViewModel = new SkillsCategoryViewModel { ID = 8, Title = "Business", Description = "To Do" };
            skill.LevelsViewModel = new SkillsLevelsViewModel { ID = 8, Level = 80, comments = "To Do" };
            skills.Add(skill);
            

            skills.ForEach(x => context.Skills.Add(x));

            //=========================================[ Experiences ]=========================================================

            var experiences = new List<ExperiencesViewModel>();
            experiences.Add(new ExperiencesViewModel
            {
                ID = 1,
                Title = "Training period",
                Company = "PSA Peugeot Citroën",
                Responsabilities = "<p>Automation of 3-D simulation result treatment for graphical Post-treatment and analysis</p> <p>Centralization of simulations result</p> <p>Graphic user interface development with MATLAB for result analysis</p>",
                StartDate = new DateTime(2011, 04, 01),
                EndDate = new DateTime(2011, 06, 30),
                link = "https://www.linkedin.com/company/260214?trk=prof-exp-company-name"
            });

            experiences.Add(new ExperiencesViewModel
            {
                ID = 2,
                Title = "Internship Engineer Integration and exploitation of services",
                Company = "Orange - France",
                Responsabilities = "<p>Maintaining in operational condition of the IT environments Technical support of the operational domains (Provisioning, Invoicing Data, Pre paid and free canal-services)</p> <p>Project manager of a supervision application deployment Web designing for indicators displaying</p>",
                StartDate = new DateTime(2011, 11, 02),
                EndDate = new DateTime(2014, 10, 30),
                link = "https://www.linkedin.com/company/1110?trk=prof-exp-company-name"
            });

            experiences.Add(new ExperiencesViewModel
            {
                ID = 3,
                Title = "Engineer Developer",
                Company = "Infoelsa",
                Responsabilities = "Bank and Insurance software (CIRIS/LIRIS) INFOELSA Champs-Elysées - FRANCE",
                StartDate = new DateTime(2014, 12, 22),
                EndDate = new DateTime(2015, 04, 30),
                link = "http://www.infoelsa.com/"
            });

            experiences.ForEach(x=>context.Experiences.Add(x));

            //=========================================[ Projects ]=========================================================

            var projects = new List<ProjectsViewModel>();

            var project = new ProjectsViewModel
            {
                ID = 1,
                Title = "The DISIC",
                Resume = "The DISIC (Inter-ministerial Directorate for Information and Communications Systems) is attached to the SGMAP (Secretariat-General for Government Modernization, under the control of the Prime Minister).",
                link = "http://www.modernisation.gouv.fr/mots-cle/disic"
            };
            project.ProjectDetail = new ProjectDetailsViewModel
            {
                ID = 1,
                Subject = "The DISIC (Interministerial Directorate for Information and Communications Systems)",
                Status = false,
                Client = "The French government supports",
                Date = new DateTime(2014, 02, 04),
                Description = "The DISIC (Inter-ministerial Directorate for Information and Communications Systems) is attached to the SGMAP (Secretariat-General for Government Modernization, under the control of the Prime Minister)..."
            };
            projects.Add(project);


            project = new ProjectsViewModel
            {
                ID = 2,
                Title = "Codsimex",
                Resume = "Codsimex is an import /Export company between Europe and Africa. The company distribute more than 10 000 referenced articles to professionals in new technologies",
                link = "http://www.codsimail.com/commercial/index2.php"
            };
            project.ProjectDetail = new ProjectDetailsViewModel
            {
                ID = 2,
                Subject = "Codsimex",
                Status = false,
                Client = "Habib Codsi",
                Date = new DateTime(2014, 06, 10),
                Description = "Codsimex is an import /Export company between Europe and Africa. The company distribute more than 10 000 referenced articles to professionals in new technologies"
            };
            projects.Add(project);


            project = new ProjectsViewModel
            {
                ID = 3,
                Title = "Portfolio",
                Resume = "Creation of a portfolio via ASP .NET MVC 5",
                link = "http://e-dago.azurewebsites.net/"
            };
            project.ProjectDetail = new ProjectDetailsViewModel
            {
                ID = 3,
                Subject = "Portfolio",
                Status = true,
                Client = "Just IT",
                Date = new DateTime(2015, 09, 15),
                Description = "Creation of a portfolio via ASP .NET MVC 5 ..."
            };
            projects.Add(project);

            projects.ForEach(x=>context.Projects.Add(x));
            //context.Projects.Add(projects[0]);

            //========================================[ Education ]============================================================

            var educations = new List<EducationViewModel>();

            educations.Add(new EducationViewModel
            {
                ID = 1,
                NbYearsToGraduate = 2,
                SchoolName = "Ville d'Avray",
                YearGraduate = new DateTime(2011, 06, 30),
                Description = "<p>DUT&nbsp;GEII - Industrial Electrical Computer Engineering&nbsp;</p>",
                link = "http://cva.u-paris10.fr/les-departements-et-leurs-formations-599365.kjsp?RH=1426843093236&RF=1426847532554"
            });

            educations.Add(new EducationViewModel
            {
                ID = 2,
                NbYearsToGraduate = 3,
                SchoolName = "ISEP",
                YearGraduate = new DateTime(2011 , 06 ,30),
                Description = "ISEP is a graduate engineering school in Electronics, Software & Computer Engineering, Signal & Image Processing, Telecommunications and Networks, founded in 1955.",
                link = "http://en.isep.fr/"
            });

            educations.Add(new EducationViewModel
            {
                ID = 3,
                NbYearsToGraduate = 1,
                SchoolName = "Just IT",
                YearGraduate = new DateTime(2015 , 09 , 10),
                Description = "Just IT are dedicated to introducing new skills and talent into the UK IT sector.TRAINING APPRENTICESHIP&nbsp; RECRUITMENT&nbsp;",
                link = "http://www.justit.co.uk/individual-training/"
            });

            educations.ForEach(x=>context.Education.Add(x));
            

            //========================================[ Pictures ]============================================================
            
            var pictures = new List<PicturesViewModel>();

            pictures.Add(new PicturesViewModel
            {
                ID = 1,
                Subject = "IUT - Ville d'Avray",
                Description = "To Do",
                FileName = "IUT-Ville-Avray.PNG",
                path = "/Content/Images/Education/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 2,
                Subject = "ISEP",
                Description = "To Do",
                FileName = "ISEP.PNG",
                path = "/Content/Images/Education/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 3,
                Subject = "Just IT",
                Description = "To Do",
                FileName = "Just-IT.PNG",
                path = "/Content/Images/Education/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 4,
                Subject = "Orange",
                Description = "To Do",
                FileName = "Orange.PNG",
                path = "/Content/Images/Experiences/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 5,
                Subject = "PSA",
                Description = "To Do",
                FileName = "PSA.PNG",
                path = "/Content/Images/Experiences/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 6,
                Subject = "Infoelsa",
                Description = "To Do",
                FileName = "Infoelsa.PNG",
                path = "/Content/Images/Experiences/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 7,
                Subject = "DISIC",
                Description = "To Do",
                FileName = "DISIC.PNG",
                path = "/Content/Images/Projects/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 8,
                Subject = "Portfolio",
                Description = "To Do",
                FileName = "Portfolio.PNG",
                path = "/Content/Images/Projects/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 9,
                Subject = "Codsimex",
                Description = "To Do",
                FileName = "Codsimex.PNG",
                path = "/Content/Images/Projects/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });

            pictures.Add(new PicturesViewModel
            {
                ID = 10,
                Subject = "Portfolio",
                Description = "To Do",
                FileName = "PortfolioModelDiagram.PNG",
                path = "/Content/Images/Projects/",
                ProjectDetail = null,
                ProjectDetailsViewModelID = null,
                Skill = null,
                SkillsViewModelID = null,
                Education = null,
                EducationViewModelID = null,
                Experience = null,
                ExperiencesViewModelID = null
            });


            pictures.ForEach(x=>context.PicturesApp.Add(x));

        }
    }
}
