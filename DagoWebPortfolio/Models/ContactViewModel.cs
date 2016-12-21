using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models
{
    public class ContactViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please give a name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please give organization or company name.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Please give an email.")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
    }
}