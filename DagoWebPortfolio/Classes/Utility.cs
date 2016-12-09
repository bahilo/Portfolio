using DagoWebPortfolio.Infrastructure;
using DagoWebPortfolio.Models;
using QCBDManagementCommon.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DagoWebPortfolio.Classes
{
    public static class Utility
    {
        static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static string getDirectory(string directory, params string[] pathElements)
        {
            var dirElements = directory.Split('/');//.Replace("/", @"\").Split('\\');
            var allPathElements = dirElements.Concat(pathElements).ToArray();
            string path = _baseDirectory;
            foreach (string pathElement in allPathElements)
            {
                if (!string.IsNullOrEmpty(pathElement))
                    path = Path.Combine(path, pathElement);
            }

            var pathChecking = path.Split('.'); // check if it is a full path file or only directory 

            if (!File.Exists(path) && !Directory.Exists(path))
            {

                if (pathChecking.Count() > 1)
                {
                    var dir = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }
                else
                    Directory.CreateDirectory(path);
            }

            return Path.GetFullPath(path);
        }

        public static string getFileFullPath(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }



        public static void populateWithDescription(EPopulateDisplay targetType, object param)
        {
            using (DBModelPortfolioContext db = new DBModelPortfolioContext())
            {
                string cultureName = CultureInfo.CurrentCulture.Name.Split('-')[0];

                switch (targetType)
                {
                    case EPopulateDisplay.Education:
                        foreach (var education in (List<EducationViewModel>)param)
                        {
                            education.Descriptions = db.Displays.Where(x => x.EducationViewModelID == education.ID && x.Lang.StartsWith(cultureName)).ToList();
                            if (education.Descriptions.Count == 0)
                                education.Descriptions = db.Displays.Where(x => x.EducationViewModelID == education.ID && x.Lang.StartsWith("en")).ToList();
                        }
                        break;
                    case EPopulateDisplay.Experiences:
                        foreach (var experience in (List<ExperiencesViewModel>)param)
                        {
                            experience.Descriptions = db.Displays.Where(x => x.ExperiencesViewModelID == experience.ID && x.Lang.StartsWith(cultureName)).ToList();
                            if (experience.Descriptions.Count == 0)
                                experience.Descriptions = db.Displays.Where(x => x.ExperiencesViewModelID == experience.ID && x.Lang.StartsWith("en")).ToList();
                        }
                        break;
                    case EPopulateDisplay.Projects:
                        foreach (var project in (List<ProjectsViewModel>)param)
                        {
                            project.Summaries = db.Displays.Where(x => x.ProjectsViewModelID == project.ID && x.Lang.StartsWith(cultureName)).ToList();
                            if (project.Summaries.Count == 0)
                                project.Summaries = db.Displays.Where(x => x.ProjectsViewModelID == project.ID && x.Lang.StartsWith("en")).ToList();
                        }
                        break;
                    case EPopulateDisplay.ProjectDetails:
                        foreach (var project in (List<ProjectsViewModel>)param)
                        {
                            project.ProjectDetail.Descriptions = db.Displays.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID && x.Lang.StartsWith(cultureName)).ToList();
                            if (project.ProjectDetail.Descriptions.Count == 0)
                                project.ProjectDetail.Descriptions = db.Displays.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID && x.Lang.StartsWith("en")).ToList();
                        }
                        break;
                    case EPopulateDisplay.Pictures:
                        foreach (var picture in (List<PicturesViewModel>)param)
                        {
                            picture.Descriptions = db.Displays.Where(x => x.PicturesViewModelID == picture.ID && x.Lang.StartsWith(cultureName)).ToList();
                            if (picture.Descriptions.Count == 0)
                                picture.Descriptions = db.Displays.Where(x => x.PicturesViewModelID == picture.ID && x.Lang.StartsWith("en")).ToList();
                        }
                        break;
                    case EPopulateDisplay.Skills:
                        foreach (var skill in (List<SkillsViewModel>)param)
                        {
                            skill.Descriptions = db.Displays.Where(x => x.SkillsViewModelID == skill.ID && x.Lang.StartsWith(cultureName)).ToList();
                            if (skill.Descriptions.Count == 0)
                                skill.Descriptions = db.Displays.Where(x => x.SkillsViewModelID == skill.ID && x.Lang.StartsWith("en")).ToList();
                        }
                        break;
                }

            }
        }



    }
}
