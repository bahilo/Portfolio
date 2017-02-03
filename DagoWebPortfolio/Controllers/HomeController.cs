using System.Linq;
using System.Data;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
using System.Data.Entity;
using System.Net;
using SendGrid;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using QCBDManagementCommon.Classes;
using System.Globalization;
using DagoWebPortfolio.Classes;
using DagoWebPortfolio.Interfaces;
using DagoWebPortfolio.Infrastructure;

namespace DagoWebPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private DBModelPortfolioContext db = new DBModelPortfolioContext();
        private IProjectsRepository _projectRepository;

        private string _culture;
        private string _cultureDefault;

        public HomeController(IProjectsRepository rep)
        {
            _culture = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault();
            _cultureDefault = "en";
            _projectRepository = rep;
            _projectRepository.setContext(db);
        }

        public ActionResult Index(bool? isMessageSent = null)
        {
            ContactViewModel ContactViewModel = new ContactViewModel();

            if (System.IO.File.Exists(Utility.getDirectory("Views", "Shared", _culture, "_Layout.cshtml")))
                ViewBag.Culture = _culture;
            else
            {
                ViewBag.Culture = _cultureDefault;
                try
                {
                    System.IO.Directory.Delete(Utility.getDirectory("Views", "Shared", _culture));
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message);
                }
            }

            loadEducation();
            loadExperiences();
            loadSkills();
            loadProjects();      
            
            try
            {
                ViewBag.Object = JsonConvert.SerializeObject(isMessageSent);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }

            // about page
            try
            {
                ViewBag.Picture = db.PicturesApp.Where(x => x.IsAbout).SingleOrDefault() ?? new PicturesViewModel();
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }

            if(System.IO.File.Exists(Utility.getDirectory("Views", "Home",_culture , "Index.cshtml")))
                return View(_culture + "/Index", ContactViewModel);
            else
                return View(_cultureDefault + "/Index", ContactViewModel);

            //try
            //{
            //    return View(_culture + "/Index");
            //}
            //catch (Exception)
            //{
            //    return View(_cultureDefault + "/Index");
            //}
        }

        private void loadExperiences()
        {
            // experiences
            var experiences = db.Experiences
                .Include(x => x.Descriptions).ToList();
            try
            {
                populateWithPicture(EPopulate.Experiences, experiences);
                Utility.populateWithDescription(EPopulate.Experiences, experiences);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            ViewBag.Experiences = getExperiencesOrderByDate(new List<SkillsViewModel> { new SkillsViewModel { Experiences = experiences } });
        }

        private void loadEducation()
        {
            // education
            List<EducationViewModel> educationList = new List<EducationViewModel>();
            try
            {
                educationList = db.Education
                .Include(x => x.Descriptions)
                .Include(x => x.Pictures).ToList();
                populateWithPicture(EPopulate.Education, educationList);
                Utility.populateWithDescription(EPopulate.Education, educationList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
                //return View("Error");
            }
            ViewBag.Education = educationList.OrderByDescending(x => x.YearGraduate).ToList();

        }

        private void loadProjects()
        {
            var projectsViewModels = db.Projects.Where(x=>x.ProjectDetail.Status)
                     .Include(x => x.Summaries)
                     .Include(x => x.ProjectDetail)
                     .Include(x => x.ProjectDetail.Descriptions)
                     .Include(x => x.ProjectDetail.Pictures).ToList();

            // projects
            //var projectSkills = db.Skills.ToList();
            //_projectRepository.populateProjectsWithProjectdetails(projectSkills);
            populateWithPicture(EPopulate.Projects, projectsViewModels);
            Utility.populateWithDescription(EPopulate.Projects, projectsViewModels);
            populateWithPicture(EPopulate.ProjectDetails, projectsViewModels);
            Utility.populateWithDescription(EPopulate.ProjectDetails, projectsViewModels);
            //_projectRepository.populateProjectsWithPicture(projectSkills);
            ViewBag.Projects = _projectRepository.getProjectsOrderByDate(new List<SkillsViewModel> { new SkillsViewModel { Projects = projectsViewModels } });

        }

        private void loadSkills()
        {
            // skills
            ViewBag.Skills = db.Skills
                .Include(x => x.Descriptions)
                .Include(x => x.LevelsViewModel)
                .Include(x => x.CategoryViewModel).ToList();
            populateWithPicture(EPopulate.Skills, ViewBag.Skills);
            Utility.populateWithDescription(EPopulate.Skills, ViewBag.Skills);
        }

        [HttpGet]
        public ActionResult Contact()
        {            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact([Bind(Include = "ID,Name,Company,Email,Phone,Comments")] ContactViewModel contactsViewModel)
        {
            ViewBag.EmailConfirmation = false;

            if (ModelState.IsValid)
            {
                try
                {
                    // smtp host config
                    Mailer mail = new Mailer("mail.e-dago.com");//("smtp.gmail.com");
                    mail.Login = "contact@e-dago.com";// "sisi.bahilo@gmail.com";
                    mail.Password = "Contact225!"; // "bahilo225";

                    mail.Subject = contactsViewModel.Name + " - " + contactsViewModel.Company;
                    mail.Body = contactsViewModel.Comments;
                    mail.Body += "<p>Phone: " + contactsViewModel.Phone + "</p>";
                    mail.Body += "<p>Email: " + contactsViewModel.Email + "</p>";
                    mail.From = mail.Login;// contactsViewModel.Email;
                    mail.FromName = contactsViewModel.Name;
                    mail.IsHtml = true;
                    mail.addAnAddress(new Dictionary<string, List<string>> {
                        { "To", new List<string> { "eric.dago.225@gmail.com" } },
                        { "Reply-To",new List<string> { contactsViewModel.Email } }
                    });
                    //mail.addAttachment(new List<string> { Utility.getDirectory("bin", "Logs", "log_2016_09.txt") });
                    mail.initialize();

                    ViewBag.EmailConfirmation = mail.send();

                    db.Contacts.Add(contactsViewModel);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { isMessageSent = ViewBag.EmailConfirmation, contact = new ContactViewModel() });
                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }   
            }

            return RedirectToAction("Index", new { isMessageSent = ViewBag.EmailConfirmation});
        }

        private void populateWithPicture(EPopulate targetType, object param)
        {
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                switch (targetType)
                {
                    case EPopulate.Education:
                        foreach (var education in (List<EducationViewModel>)param)
                        {
                            education.Pictures = db.PicturesApp.Where(x => x.EducationViewModelID == education.ID).ToList();
                        }
                        break;
                    case EPopulate.Experiences:
                        foreach (var experience in (List<ExperiencesViewModel>)param)
                        {
                            experience.Pictures = db.PicturesApp.Where(x => x.ExperiencesViewModelID == experience.ID).ToList();
                        }
                        break;
                    case EPopulate.Projects:
                        foreach (var project in (List<ProjectsViewModel>)param)
                        {
                            project.ProjectDetail.Pictures = db.PicturesApp.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID).ToList();
                        }
                        break;
                    //case EPopulateDisplay.Skills:
                        //foreach (var skill in (List<SkillsViewModel>)param)
                        //{
                        //    skill.Pictures = db.PicturesApp.Where(x => x.SkillsViewModelID == skill.ID).ToList();
                        //}
                        //break;
                }
            }
        }

        
        private IEnumerable<ExperiencesViewModel> getExperiencesOrderByDate(List<SkillsViewModel> skills)
        {
            var experiencesListFromSkills = (from e in (from d in skills select d.Experiences).Distinct() select e).ToList();

            List<ExperiencesViewModel> experiencesListFinal = new List<ExperiencesViewModel>();
            foreach (var experiencesList in experiencesListFromSkills)
            {
                foreach (var experience in experiencesList.ToList())
                {
                    if (experience != null)
                    {
                        experiencesListFinal.Add(experience);
                    }
                }
            }
            var experiencesListFinalDistict = experiencesListFinal.Distinct().OrderByDescending(x => x.EndDate).ToList();

            return experiencesListFinalDistict;
        }


    }

}