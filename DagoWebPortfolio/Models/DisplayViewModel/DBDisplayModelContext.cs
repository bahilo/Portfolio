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


    public class MockInitializerDisplay : DropCreateDatabaseAlways<DBDisplayModelContext>
    {
        protected override void Seed(DBDisplayModelContext context)
        {
            base.Seed(context);

            //========================================[ Displays ]============================================================

            var displays = new List<DisplayViewModel>();

            displays.Add(new DisplayViewModel
            {
                ID = 1,
                lang = "EN"
            });


            displays[0].AboutView = new DisplayViewAbout
            {
                ID = 1,
                HeadZone1 = "<p>E. DAGO PORTFOLIO</p>",
                HeadZone2 = "<p>ERIC DAGO</p>",
                HeadZone3 = "<p>I am A C#&nbsp;Developer</p>",

                BodyZone1 = "<p>Having the fundamentals of programming well structured, my dream is not only to become a better programmer, but also to build my lifestyle around it.</p>",
                BodyZone2 = "<p>Seeing progress and appreciation in the work I do makes me more ambitious and keen to offer more.</p>",
                BodyZone3 = "<p>Seeing all the opportunities that today's technologies offer gives me great motivation to be an important part of the process.</p>",
                BodyZone4 = "<p>I think quality products not only make you a better programmer but seeing happy clients in the end, gives great satisfaction.</p>",
                BodyZone5 = "<p>Essex England</p>",

                FileName = "photo_id1.jpg",
                Name = "ERIC",
                Path = "/Content/Images/Welcome/"
            };

            displays[0].WelcomeView = new DisplayViewWelcome
            {
                ID = 1,
                Zone1 = "<h1>Hi, I'M ERIC</h1>",
                Zone2 = "<p>Welcome to my Portfolio</p>",
                Zone3 = "<p>Learn more</p>",
                FileName = "photo_id1.jpg",
                Name = "ERIC",
                Path = "/Content/Images/Welcome/"                

            };

            displays.ForEach(x => context.Displays.Add(x));

        }
    }
}