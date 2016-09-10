using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class PicturesViewModel
    {
        public int ID { get; set; }        

        [Required][Display(Name ="File Subject")]
        public string Subject { get; set; }    
            
        public string path { get; set; }
        public string FileName { get; set; } 

        public string link { get; set; }

        [Display(Name = "About Page")]
        public bool IsAbout { get; set; }
        [Display(Name = "Welcome Page")]
        public bool IsWelcome { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        [Display(Name="Link To Project")]
        public int? ProjectDetailsViewModelID { get; set; }

        [Display(Name = "Link To Education")]
        public int? EducationViewModelID { get; set; }

        [Display(Name = "Link To Experience")]
        public int? ExperiencesViewModelID { get; set; }

        [Display(Name = "Link To Skills")]
        public int? SkillsViewModelID { get; set; }

        [ForeignKey("ProjectDetailsViewModelID")]
        public virtual ProjectDetailsViewModel ProjectDetail { get; set; }

        [ForeignKey("EducationViewModelID")]
        public virtual EducationViewModel Education { get; set; }

        [ForeignKey("ExperiencesViewModelID")]
        public virtual ExperiencesViewModel Experience { get; set; }

        [ForeignKey("SkillsViewModelID")]
        public virtual SkillsViewModel Skill { get; set; }


        public PicturesViewModel()
        {
            ProjectDetail = new ProjectDetailsViewModel();
            Education = new EducationViewModel();
            Experience = new ExperiencesViewModel();
            Skill = new SkillsViewModel();/**/
        }
    }
}
