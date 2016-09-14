﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class EducationModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must specify a School Name.")]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "You must specify Year Of Graduation.")]
        [DataType(DataType.Date)]
        [Display(Name = "Year Of Graduation")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime YearGraduate { get; set; }

        [Display(Name = "Number Of Years")]
        public int NbYearsToGraduate { get; set; }

        public string link { get; set; }

        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Description { get; set; }

        public virtual ICollection<PicturesViewModel> Pictures { get; set; }

        public EducationModel()
        {
            Pictures = new HashSet<PicturesViewModel>();
            this.YearGraduate = DateTime.Now;
        }
    }
}