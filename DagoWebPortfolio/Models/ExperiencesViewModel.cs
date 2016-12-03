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
    public class ExperiencesViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Position")]
        public string Title { get; set; }

        public string Company { get; set; }

        [Required(ErrorMessage = "You must specify a start date.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "You must specify a end date.")]
        [DataType(DataType.Date)]
        [Display(Name = "End date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public string link { get; set; }

        public virtual ICollection<SkillsViewModel> Skills { get; set; }
        public virtual ICollection<PicturesViewModel> Pictures { get; set; }
        public virtual ICollection<DisplayViewModel> Descriptions { get; set; }
        
    }
}
