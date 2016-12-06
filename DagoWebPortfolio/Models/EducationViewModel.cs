using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class EducationViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="You must specify a School Name.")]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage = "You must specify Year Of Graduation.")]
        [DataType(DataType.Date)]
        [Display(Name ="Year Of Graduation")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime YearGraduate { get; set; }

        [Display(Name = "Number Of Years")]
        public int NbYearsToGraduate { get; set; }

        public string link { get; set; }

        public virtual ICollection<PicturesViewModel> Pictures { get; set; }
        public virtual ICollection<DisplayViewModel> Descriptions { get; set; }
        public virtual ICollection<TechnoEnvironmentsViewModel> TechnoEnv { get; set; }

    }
    
}
