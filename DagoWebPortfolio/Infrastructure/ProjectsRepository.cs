using DagoWebPortfolio.Interfaces;
using DagoWebPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace DagoWebPortfolio.Infrastructure
{
    public class ProjectsRepository : IProjectsRepository
    {
        DBModelPortfolioContext db;

        public void setContext(DBModelPortfolioContext dbContext)
        {
            db = dbContext;
        }

        //---------------------------------------------------------------------------------------
        //                          Populate project
        //---------------------------------------------------------------------------------------
        
        /// <summary>
        /// Populate project with the attached detail
        /// </summary>
        /// <param name="skills"></param>
        public void populateProjectsWithProjectdetails(List<SkillsViewModel> skills)
        {
            foreach (var skill in skills)
            {
                foreach (var project in skill.Projects)
                {
                    //project = db.Projects.Where().Include("ProjectDetail");
                    project.ProjectDetail = db.Projects.Where(x => x.ID == project.ID).Include("ProjectDetail").Select(x=>x.ProjectDetail).Single();
                    //project.ProjectDetail = db.DetailsProject.Where(x => x.ID == project.ProjectDetail.ID).Single();
                }
            }
        }

        /// <summary>
        /// Polulate project with the attached pictures
        /// </summary>
        /// <param name="skills"></param>
        public void populateProjectsWithPicture(List<SkillsViewModel> skills)
        {
            foreach (var skill in skills)
            {
                foreach (var project in skill.Projects)
                {
                    project.ProjectDetail.Pictures = db.PicturesApp.Where(x => x.ProjectDetailsViewModelID == project.ProjectDetail.ID).ToList();
                }
            }
        }



        //---------------------------------------------------------------------------------------
        //                          Update project
        //---------------------------------------------------------------------------------------


        /// <summary>
        /// Update project in database
        /// </summary>
        /// <param name="project"></param>
        /// <param name="listSkillOfProjectsId"></param>
        /// <param name="isSkillSelected"></param>
        public void addOrUpdateSkillWithObjects(ProjectsViewModel project, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected)
        {
            // Project Skills handling
            List<int> listAddedSkillsId = new List<int>();
            if (isSkillSelected != null)
            {
                int id;
                SkillsViewModel newSkill;
                for (int i = 0; i < isSkillSelected.Count(); i++)
                {
                    string isSelected = isSkillSelected.ElementAt(i).Split('-').ToList()[0];
                    string isSelectedId = isSkillSelected.ElementAt(i).Split('-').ToList()[1];
                    id = Int32.Parse(isSelectedId);
                    newSkill = db.Skills.Where(y => y.ID == id).Include("CategoryViewModel").Include("LevelsViewModel").DefaultIfEmpty().Single();
                    if (isSelected == "true")
                    {
                        project.Skills.Add(newSkill);
                        listAddedSkillsId.Add(id);
                    }
                }
            }

            // removing uncheck Projects skills
            if (listSkillOfProjectsId != null)
            {
                foreach (var skillIdString in listSkillOfProjectsId)
                {
                    var skillId = Int32.Parse(skillIdString);
                    if ((listAddedSkillsId != null && !listAddedSkillsId.Contains(skillId)) || listAddedSkillsId == null)
                    {
                        var skillToRemove = db.Skills.Where(x => x.ID == skillId).Include("CategoryViewModel").Include("LevelsViewModel").First();
                        project.Skills.Remove(skillToRemove);
                    }
                }
            }

        }


        /// <summary>
        /// Save project from form into database 
        /// </summary>
        /// <param name="formDataDictionary"></param>
        /// <param name="outputProjectModel"></param>
        public void populateDBWithDataFromForm(Dictionary<string, object> formDataDictionary, ProjectsViewModel outputProjectModel)
        {
            var id = Int32.Parse((string)formDataDictionary["projectDetailID"]);

            addOrUpdateSkillWithObjects(((ProjectsViewModel)formDataDictionary["projectsViewModel"]), (IEnumerable<string>)formDataDictionary["listSkillOfProjectsId"], (IEnumerable<string>)formDataDictionary["isSkillSelected"]);

            outputProjectModel.link = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).link;
            //outputProjectModel.Resume = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).Resume;
            outputProjectModel.Title = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).Title;
            outputProjectModel.Skills = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).Skills;

            //ProjectDetailsViewModel newProjectDetail = new ProjectDetailsViewModel();
            outputProjectModel.ProjectDetail.Subject = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).ProjectDetail.Subject;
            outputProjectModel.ProjectDetail.Status = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).ProjectDetail.Status;
            outputProjectModel.ProjectDetail.Descriptions = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).ProjectDetail.Descriptions;
            outputProjectModel.ProjectDetail.Date = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).ProjectDetail.Date;
            outputProjectModel.ProjectDetail.Client = ((ProjectsViewModel)formDataDictionary["projectsViewModel"]).ProjectDetail.Client;

            //outputProjectModel.ProjectDetail = newProjectDetail;

        }

        //---------------------------------------------------------------------------------------
        //                          Delete project
        //---------------------------------------------------------------------------------------

        /// <summary>
        /// Delete project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Server"></param>
        public void deleteProject(int id, HttpServerUtilityBase Server)
        {
            var projectsViewModelWithIncludes = db.Projects.Include("ProjectDetail");
            var projectsViewModel = projectsViewModelWithIncludes.First(x => x.ID == id);
            var detailWithIncludes = db.DetailsProject.Include("Pictures").Where(x => x.ID == projectsViewModel.ProjectDetail.ID).Single();
            try
            {
                var listPictures = detailWithIncludes.Pictures.Where(x => x.ProjectDetailsViewModelID == projectsViewModel.ProjectDetail.ID).DefaultIfEmpty();

                foreach (var picture in listPictures)
                {
                    deletePicture(picture, Server);
                }
                db.DetailsProject.Remove(db.DetailsProject.First(x => x.ID == projectsViewModel.ProjectDetail.ID));
                db.Projects.Remove(projectsViewModel);
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }

                throw raise;
            }
        }


        /// <summary>
        /// Delete project picture
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="Server"></param>
        public void deletePicture(PicturesViewModel picture, HttpServerUtilityBase Server)
        {
            if (picture != null)
            {

                string[] savedFiles;
                string origineFileWithPath;

                savedFiles = System.IO.Directory.GetFiles(Server.MapPath("~" + picture.path));
                origineFileWithPath = Server.MapPath("~" + picture.path) + picture.FileName;

                foreach (var f in savedFiles)
                {
                    if (origineFileWithPath.Equals(f))
                        System.IO.File.Delete(f);
                }

                db.PicturesApp.Remove(picture);
            }
        }


        /// <summary>
        /// Order project by date 
        /// </summary>
        /// <param name="skills"></param>
        /// <returns></returns>
        public IEnumerable<ProjectsViewModel> getProjectsOrderByDate(List<SkillsViewModel> skills)
        {
            var projectsListFromSkills = (from e in (from d in skills select d.Projects.OrderBy(x => x.ProjectDetail.Date)).Distinct() select e).ToList();

            List<ProjectsViewModel> projectsListFinal = new List<ProjectsViewModel>();
            foreach (var projectsList in projectsListFromSkills)
            {
                foreach (var project in projectsList.ToList())
                {
                    if (project != null)
                    {
                        projectsListFinal.Add(project);
                    }
                }
            }
            var projectsListFinalDistict = projectsListFinal.Distinct().OrderByDescending(x=>x.ProjectDetail.Date).ToList();

            return projectsListFinalDistict;
        }      


    }
}
