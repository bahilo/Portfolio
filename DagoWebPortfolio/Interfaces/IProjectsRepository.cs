using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DagoWebPortfolio.Models;
using System.Web;

namespace DagoWebPortfolio.Interfaces
{
    public interface IProjectsRepository
    {
        void populateProjectsWithProjectdetails(List<SkillsViewModel> skills);
        void populateProjectsWithPicture(List<SkillsViewModel> skills);
        IEnumerable<ProjectsViewModel> getProjectsOrderByDate(List<SkillsViewModel> skills);
        void setContext(DBModelPortfolioContext dbContext);
        void populateDBWithDataFromForm(ProjectsViewModel projectsViewModel, string projectDetailID, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected);
        void deletePicture(PicturesViewModel picture, HttpServerUtilityBase Server);
        void deleteProject(int id, HttpServerUtilityBase Server);
        void addOrUpdateSkillWithObjects(ProjectsViewModel project, IEnumerable<string> listSkillOfProjectsId, IEnumerable<string> isSkillSelected);
    }
}
