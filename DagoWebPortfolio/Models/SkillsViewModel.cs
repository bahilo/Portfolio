﻿
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
    public class SkillsViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must specify a skill name.")]
        [Display(Name="Skill Name")]
        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        public virtual SkillsLevelsViewModel LevelsViewModel { get; set; }
        public virtual SkillsCategoryViewModel CategoryViewModel { get; set; }
        public virtual ICollection<ProjectsViewModel> Projects { get; set; }
        public virtual ICollection<ExperiencesViewModel> Experiences { get; set; }
        public virtual ICollection<PicturesViewModel> Pictures { get; set; }
        
        
        public SkillsViewModel()
        {
            Experiences = new HashSet<ExperiencesViewModel>();
            Projects = new HashSet<ProjectsViewModel>();
            CategoryViewModel = new SkillsCategoryViewModel();
            LevelsViewModel = new SkillsLevelsViewModel();
            Pictures = new HashSet<PicturesViewModel>();
        }
        //public int LevelsId { get; set; }
        //public int ExperiencesId { get; set; }
        //public int ProjectsId { get; set; }
    }
    
}
