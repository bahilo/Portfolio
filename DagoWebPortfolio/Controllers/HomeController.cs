using System.Linq;
using System.Data;
using System.Web.Mvc;
using DagoWebPortfolio.Models;
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
                

            // education
            List<EducationViewModel> educationList = new List<EducationViewModel>();
            try
            {
                educationList = db.Education.Include("Pictures").Include("Descriptions").ToList();
                populateWithPicture(EPopulateDisplay.Education, educationList);
                Utility.populateWithDescription(EPopulateDisplay.Education, educationList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
                return View("Error");
            }
            ViewBag.Education = educationList.OrderByDescending(x => x.YearGraduate).ToList();

            // projects
            var projectSkills = db.Skills.ToList();
            _projectRepository.populateProjectsWithProjectdetails(projectSkills);
            populateWithPicture(EPopulateDisplay.Projects, projectSkills.SelectMany(x => x.Projects).ToList());
            Utility.populateWithDescription(EPopulateDisplay.Projects, projectSkills.SelectMany(x => x.Projects).ToList());
            populateWithPicture(EPopulateDisplay.ProjectDetails, projectSkills.SelectMany(x => x.Projects).ToList());
            Utility.populateWithDescription(EPopulateDisplay.ProjectDetails, projectSkills.SelectMany(x => x.Projects).ToList());
            //_projectRepository.populateProjectsWithPicture(projectSkills);
            ViewBag.Projects = _projectRepository.getProjectsOrderByDate(projectSkills);

            // skills
            ViewBag.Skills = db.Skills.Include("LevelsViewModel").Include("CategoryViewModel").ToList();
            populateWithPicture(EPopulateDisplay.Skills, ViewBag.Skills);
            Utility.populateWithDescription(EPopulateDisplay.Skills, ViewBag.Skills);


            // experiences
            var experienceSkills = db.Skills.Include("Experiences").Include("LevelsViewModel").Include("CategoryViewModel").ToList();
            try
            {
                populateWithPicture(EPopulateDisplay.Experiences, experienceSkills.SelectMany(x => x.Experiences).ToList());
                Utility.populateWithDescription(EPopulateDisplay.Experiences, experienceSkills.SelectMany(x=>x.Experiences).ToList());
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            ViewBag.Experiences = getExperiencesOrderByDate(experienceSkills);
            
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
                return View(_culture + "/Index");
            else
                return View(_cultureDefault + "/Index");
            
            //try
            //{
            //    return View(_culture + "/Index");
            //}
            //catch (Exception)
            //{
            //    return View(_cultureDefault + "/Index");
            //}
        }

        public ActionResult _Welcome()
        {
            var picture = new PicturesViewModel();
            try
            {
                picture = db.PicturesApp.Where(x => x.IsWelcome).SingleOrDefault() ?? new PicturesViewModel();
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }

            try
            {
                return View(_culture + "/_Welcome", picture);
            }
            catch (Exception)
            {
                return View(_cultureDefault + "/_Welcome", picture);
            }
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
                        { "To", new List<string> { "joel.dago@yahoo.fr" } },
                        { "Reply-To",new List<string> { contactsViewModel.Email } }
                    });
                    //mail.addAttachment(new List<string> { Utility.getDirectory("bin", "Logs", "log_2016_09.txt") });
                    mail.initialize();

                    ViewBag.EmailConfirmation = mail.send();

                    db.Contacts.Add(contactsViewModel);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Log.write(ex.Message, "ERR");
                }   
            }
            return RedirectToAction("Index", new { isMessageSent = ViewBag.EmailConfirmation });
            //return RedirectToAction("Index", new { page = "Contact" });
            //return View(contactsViewModel);
        }

        private void populateWithPicture(EPopulateDisplay targetType, object param)
        {
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                switch (targetType)
                {
                    case EPopulateDisplay.Education:
                        foreach (var education in (List<EducationViewModel>)param)
                        {
                            education.Pictures = db.PicturesApp.Where(x => x.EducationViewModelID == education.ID).ToList();
                        }
                        break;
                    case EPopulateDisplay.Experiences:
                        foreach (var experience in (List<ExperiencesViewModel>)param)
                        {
                            experience.Pictures = db.PicturesApp.Where(x => x.ExperiencesViewModelID == experience.ID).ToList();
                        }
                        break;
                    case EPopulateDisplay.Projects:
                        foreach (var project in (List<ProjectsViewModel>)param)
                        {
                            project.ProjectDetail.Pictures = db.PicturesApp.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID).ToList();
                        }
                        break;
                    case EPopulateDisplay.Skills:
                        foreach (var skill in (List<SkillsViewModel>)param)
                        {
                            skill.Pictures = db.PicturesApp.Where(x => x.SkillsViewModelID == skill.ID).ToList();
                        }
                        break;
                }
            }
        }

        //private void populateWithDescription( EPopulateDisplay targetType, object param)
        //{
        //    using (DBModelPortfolioContext db = new DBModelPortfolioContext())
        //    {
        //        string cultureName = CultureInfo.CurrentCulture.Name.Split('-')[0];

        //        switch (targetType)
        //        {
        //            case EPopulateDisplay.Education:
        //                foreach (var education in (List<EducationViewModel>)param)
        //                {
        //                    education.Descriptions = db.Displays.Where(x => x.EducationViewModelID == education.ID && x.Lang.StartsWith(cultureName)).ToList();
        //                    if (education.Descriptions.Count == 0)
        //                        education.Descriptions = db.Displays.Where(x => x.EducationViewModelID == education.ID && x.Lang.StartsWith("en")).ToList();
        //                }
        //                break;
        //            case EPopulateDisplay.Experiences:
        //                foreach (var experience in (List<ExperiencesViewModel>)param)
        //                {
        //                    experience.Descriptions = db.Displays.Where(x => x.ExperiencesViewModelID == experience.ID && x.Lang.StartsWith(cultureName)).ToList();
        //                    if (experience.Descriptions.Count == 0)
        //                        experience.Descriptions = db.Displays.Where(x => x.ExperiencesViewModelID == experience.ID && x.Lang.StartsWith("en")).ToList();
        //                }
        //                break;
        //            case EPopulateDisplay.Projects:
        //                foreach (var project in (List<ProjectsViewModel>)param)
        //                {
        //                    project.Summaries = db.Displays.Where(x => x.ProjectsViewModelID == project.ID && x.Lang.StartsWith(cultureName)).ToList();
        //                    if (project.Summaries.Count == 0)
        //                        project.Summaries = db.Displays.Where(x => x.ProjectsViewModelID == project.ID && x.Lang.StartsWith("en")).ToList();
        //                }
        //                break;
        //            case EPopulateDisplay.ProjectDetails:
        //                foreach (var project in (List<ProjectsViewModel>)param)
        //                {
        //                    project.ProjectDetail.Descriptions = db.Displays.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID && x.Lang.StartsWith(cultureName)).ToList();
        //                    if (project.ProjectDetail.Descriptions.Count == 0)
        //                        project.ProjectDetail.Descriptions = db.Displays.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID && x.Lang.StartsWith("en")).ToList();
        //                }
        //                break;
        //            case EPopulateDisplay.Pictures:
        //                foreach (var picture in (List<PicturesViewModel>)param)
        //                {
        //                    picture.Descriptions = db.Displays.Where(x => x.PicturesViewModelID == picture.ID && x.Lang.StartsWith(cultureName)).ToList();
        //                    if (picture.Descriptions.Count == 0)
        //                        picture.Descriptions = db.Displays.Where(x => x.PicturesViewModelID == picture.ID && x.Lang.StartsWith("en")).ToList();
        //                }
        //                break;
        //            case EPopulateDisplay.Skills:
        //                foreach (var skill in (List<SkillsViewModel>)param)
        //                {
        //                    skill.Descriptions = db.Displays.Where(x => x.SkillsViewModelID == skill.ID && x.Lang.StartsWith(cultureName)).ToList();
        //                    if (skill.Descriptions.Count == 0)
        //                        skill.Descriptions = db.Displays.Where(x => x.SkillsViewModelID == skill.ID && x.Lang.StartsWith("en")).ToList();
        //                }
        //                break;
        //        }
                
        //    }
        //}

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