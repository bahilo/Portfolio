using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models.DisplayViewModel
{
    public class DisplayViewAbout
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }

        [Display(Name = "Head Logo")]
        [AllowHtml]
        public string HeadZone1 { get; set; }

        [Display(Name = "Head Name")]
        [AllowHtml]
        public string HeadZone2 { get; set; }

        [Display(Name = "Head Description")]
        [AllowHtml]
        public string HeadZone3 { get; set; }

        [Display(Name = "Body Quote")]
        [AllowHtml]
        public string BodyZone1 { get; set; }

        [Display(Name = "Body Ambition")]
        [AllowHtml]
        public string BodyZone2 { get; set; }

        [Display(Name = "Body Motivation")]
        [AllowHtml]
        public string BodyZone3 { get; set; }

        [Display(Name = "Body Quality")]
        [AllowHtml]
        public string BodyZone4 { get; set; }

        [Display(Name = "Body Address")]
        [AllowHtml]
        public string BodyZone5 { get; set; }

        /*public int DisplayViewModelID { get; set; }

        [ForeignKey("DisplayViewModelID")]
        public DisplayViewModel Display { get; set; }

        public DisplayViewAbout()
        {
            Display = new DisplayViewModel();
        }*/
    }
}