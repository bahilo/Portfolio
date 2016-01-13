
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class ProjectsViewModel
    {
        public int ID { get; set; }   
              
        [Display(Name = "Project Name")]
        public string Title { get; set; }

        public string link { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Resume { get; set; }
        

        public virtual ICollection<SkillsViewModel> Skills { get; set; }
        public virtual ProjectDetailsViewModel ProjectDetail { get; set; }

        public ProjectsViewModel()
        {
            Skills = new HashSet<SkillsViewModel>();
            ProjectDetail = new ProjectDetailsViewModel();
        }
    }

}
