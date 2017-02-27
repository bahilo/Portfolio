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



        public static void populateWithDescription(object param, string cultureName, EPopulate targetType)
        {
            
            switch (targetType)
            {
                case EPopulate.Education:
                    foreach (var education in (List<EducationViewModel>)param)
                    {
                        var descriptions = education.Descriptions.Where(x => x.Lang.StartsWith(cultureName)).ToList();
                        if (descriptions.Count == 0)
                            descriptions = education.Descriptions.Where(x => x.Lang.StartsWith("en")).ToList();

                        education.Descriptions = descriptions;
                    }
                    break;
                case EPopulate.Experiences:
                    foreach (var experience in (List<ExperiencesViewModel>)param)
                    {
                        var descriptions = experience.Descriptions.Where(x => x.Lang.StartsWith(cultureName)).ToList();
                        if (descriptions.Count == 0)
                            descriptions = experience.Descriptions.Where(x => x.Lang.StartsWith("en")).ToList();

                        experience.Descriptions = descriptions;
                    }
                    break;
                case EPopulate.Projects:
                    foreach (var project in (List<ProjectsViewModel>)param)
                    {
                        var descriptions = project.Summaries.Where(x => x.Lang.StartsWith(cultureName)).ToList();
                        if (descriptions.Count == 0)
                            descriptions = project.Summaries.Where(x => x.Lang.StartsWith("en")).ToList();

                        project.Summaries = descriptions;
                    }
                    break;
                case EPopulate.ProjectDetails:
                    foreach (var project in (List<ProjectsViewModel>)param)
                    {
                        var descriptions = project.ProjectDetail.Descriptions.Where(x => x.Lang.StartsWith(cultureName)).ToList();
                        if (descriptions.Count == 0)
                            descriptions = project.ProjectDetail.Descriptions.Where(x => x.Lang.StartsWith("en")).ToList();

                        project.ProjectDetail.Descriptions = descriptions;
                    }
                    break;
                case EPopulate.Pictures:
                    foreach (var picture in (List<PicturesViewModel>)param)
                    {
                        var descriptions = picture.Descriptions.Where(x => x.Lang.StartsWith(cultureName)).ToList();
                        if (descriptions.Count == 0)
                            descriptions = picture.Descriptions.Where(x => x.Lang.StartsWith("en")).ToList();

                        picture.Descriptions = descriptions;
                    }
                    break;
                case EPopulate.Skills:
                    foreach (var skill in (List<SkillsViewModel>)param)
                    {
                        var descriptions = skill.Descriptions.Where(x => x.Lang.StartsWith(cultureName)).ToList();
                        if (descriptions.Count == 0)
                            descriptions = skill.Descriptions.Where(x => x.Lang.StartsWith("en")).ToList();

                        skill.Descriptions = descriptions;
                    }
                    break;
            }
        }



    }
}
