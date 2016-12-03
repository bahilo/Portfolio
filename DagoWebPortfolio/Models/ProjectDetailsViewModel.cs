using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class ProjectDetailsViewModel
    {
        public int ID { get; set; }     
           
        [Required(ErrorMessage = "You must fill up the Subject field.")]
        [Display(Name ="Detail Subject")]
        public string Subject { get; set; }  
              
        [Display(Name = "Active Project")]
        public bool Status { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] 
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } 
               
        [Display(Name = "Client Name")] 
        public string Client { get; set; }

        public virtual ICollection<PicturesViewModel> Pictures { get; set; }
        public virtual ICollection<DisplayViewModel> Descriptions { get; set; }
        
    }
}
