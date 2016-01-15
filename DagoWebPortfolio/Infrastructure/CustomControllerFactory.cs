using DagoWebPortfolio.Controllers;
using DagoWebPortfolio.Models;
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
            if (controllerName.ToLower().StartsWith("projects"))
            {
                //using (DBModelPortfolioContext db = new DBModelPortfolioContext())
                //{
                ProjectsRepository rep = new ProjectsRepository();
                var controller = new ProjectsController(rep);
                return controller;
                //}             
            }

            var defaultController = new DefaultControllerFactory();
            return defaultController.CreateController(requestContext, controllerName);

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
