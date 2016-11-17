using DagoWebPortfolio.Controllers;
using DagoWebPortfolio.Models;
using QCBDManagementCommon.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace DagoWebPortfolio.Infrastructure
{
    public class CustomControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            ProjectsRepository rep = new ProjectsRepository();
            IController controller = default(IController);
            string language = "en-GB";
            System.Web.HttpRequestBase Request = requestContext.HttpContext.Request;

            if (Request.UserLanguages != null)
                language = Request.UserLanguages[0];

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            
            try
            {
                if (controllerName.ToLower().StartsWith("projects"))
                    controller = new ProjectsController(rep);
                else if (controllerName.ToLower().StartsWith("home"))
                    controller = new HomeController(rep);
                else
                    controller = new DefaultControllerFactory().CreateController(requestContext, controllerName);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }   

            return controller;
            
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            var disposable = controller as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
