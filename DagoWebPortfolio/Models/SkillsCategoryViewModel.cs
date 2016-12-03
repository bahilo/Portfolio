
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
    public class SkillsCategoryViewModel
    {
        public int ID { get; set; }

        //[Required(ErrorMessage = "[Category] You must give a Title.")]
        public string Title { get; set; }

        [AllowHtml]
        public string Description { get; set; }
    }
}
