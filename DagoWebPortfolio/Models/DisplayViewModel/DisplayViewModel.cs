using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DagoWebPortfolio.Models.DisplayViewModel
{
    public class DisplayViewModel
    {
        public int ID { get; set; }
        public string lang { get; set; }

        public DisplayViewAbout AboutView { get; set; }
        public DisplayViewWelcome WelcomeView { get; set; }

    }
}