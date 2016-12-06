using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class TechnoEnvironmentsViewModel
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public string Category { get; set; }
        public string path { get; set; }
        public string FileName { get; set; }

        public string link { get; set; }

        public virtual ICollection<DisplayViewModel> Descriptions { get; set; }

        [Display(Name = "Link To Project")]
        public int? ProjectsViewModelID { get; set; }

        [Display(Name = "Link To Project detail")]
        public int? ProjectDetailsViewModelID { get; set; }

        [Display(Name = "Link To Education")]
        public int? EducationViewModelID { get; set; }

        [Display(Name = "Link To Experience")]
        public int? ExperiencesViewModelID { get; set; }

        [Display(Name = "Link To Skills")]
        public int? SkillsViewModelID { get; set; }

        [Display(Name = "Link To Pictures")]
        public int? PicturesViewModelID { get; set; }

        [ForeignKey("ProjectsViewModelID")]
        public virtual ProjectsViewModel Project { get; set; }

        [ForeignKey("ProjectDetailsViewModelID")]
        public virtual ProjectDetailsViewModel ProjectDetail { get; set; }

        [ForeignKey("EducationViewModelID")]
        public virtual EducationViewModel Education { get; set; }

        [ForeignKey("ExperiencesViewModelID")]
        public virtual ExperiencesViewModel Experience { get; set; }

        [ForeignKey("SkillsViewModelID")]
        public virtual SkillsViewModel Skill { get; set; }

        [ForeignKey("PicturesViewModelID")]
        public virtual PicturesViewModel Picture { get; set; }
    }
}