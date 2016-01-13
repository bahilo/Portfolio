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
    public class SkillsLevelsViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "You must give a Level.")]
        public int Level { get; set; }

        [AllowHtml]
        public string comments { get; set; }
        //[Display(Name = "Skill Involves")]
        //public int SkillsRefId { get; set; }

        //[ForeignKey("SkillsRefId")]        
        //public int SkillsViewModelID { get; set; }

        //[ForeignKey("SkillsViewModelID")]
        //public virtual SkillsViewModel SkillsViewModel { get; set; }
                                           /* public LevelsViewModel()
                                            {
                                                Skills = new SkillsViewModel();
                                            }*/

    }
    
}
