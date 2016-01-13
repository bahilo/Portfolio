using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models.DisplayViewModel
{
    public class DisplayViewWelcome
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }

        [Display(Name = "Salutation")]
        [AllowHtml]
        public string Zone1 { get; set; }

        [Display(Name = "Welcome")]
        [AllowHtml]
        public string Zone2 { get; set; }

        [Display(Name = "Link More")]
        [AllowHtml]
        public string Zone3 { get; set; }

        /*public int DisplayViewModelID { get; set; }

        [ForeignKey("DisplayViewModelID")]
        public DisplayViewModel Display { get; set; }

        public DisplayViewWelcome()
        {
            Display = new DisplayViewModel();
        }*/
    }
}