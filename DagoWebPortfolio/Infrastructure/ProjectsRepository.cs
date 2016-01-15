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
        

        public void populateProjectsWithProjectdetails(List<SkillsViewModel> skills)
        {
            foreach (var skill in skills)
            {
                foreach (var project in skill.Projects)
                {
                    project.ProjectDetail = db.DetailsProject.Where(x => x.ID == project.ProjectDetail.ID).DefaultIfEmpty().Single();
                }
            }
        }

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
            var projectsListFinalDistict = projectsListFinal.Distinct().Reverse().ToList();

            return projectsListFinalDistict;
        }


        public void populateDBWithDataFromForm(ProjectsViewModel projectsViewModel, string projectDetailID, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected)
        {
            var id = Int32.Parse(projectDetailID);
            var origineParentDetail = db.DetailsProject.First(x => x.ID == id);
            origineParentDetail.Subject = projectsViewModel.ProjectDetail.Subject;
            origineParentDetail.Status = projectsViewModel.ProjectDetail.Status;
            origineParentDetail.Description = projectsViewModel.ProjectDetail.Description;
            origineParentDetail.Date = projectsViewModel.ProjectDetail.Date;
            projectsViewModel.ProjectDetail = origineParentDetail;

            addOrUpdateSkillWithObjects(projectsViewModel, listSkillOfProjectsId, isSkillSelected);
            
            var origineProject = db.Projects.Where(x => x.ID == projectsViewModel.ID).Include("ProjectDetail").DefaultIfEmpty().Single();
            
            
            //try
            //{
            //    origineProject.ProjectDetail = projectsViewModel.ProjectDetail;
            //    origineProject.link = projectsViewModel.link;
            //    origineProject.Resume = projectsViewModel.Resume;
            //    origineProject.Title = projectsViewModel.Title;
            //    origineProject.Skills = projectsViewModel.Skills;

            //    db.Entry(origineProject).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    Exception raise = dbEx;
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            string message = string.Format("Property: {0} Error: {1}",
            //                                    validationError.PropertyName,
            //                                    validationError.ErrorMessage);
            //            raise = new InvalidOperationException(message, raise);
            //        }
            //    }

            //    throw raise;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }





    }
}
