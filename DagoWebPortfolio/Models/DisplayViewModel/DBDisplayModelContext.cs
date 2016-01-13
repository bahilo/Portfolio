using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DagoWebPortfolio.Models.DisplayViewModel
{
    public class DBDisplayModelContext : DbContext
    {
        public DbSet<DisplayViewAbout> DisplayAbout { get; set; }
        public DbSet<DisplayViewWelcome> DisplayWelcome { get; set; }
        public DbSet<DisplayViewModel> Displays { get; set; }

    }
}