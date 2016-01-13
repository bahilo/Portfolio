using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DagoWebPortfolio.Models.CustomHelpers
{
    public static class HtmlDisplayHelper
    {
        public static IHtmlString DisplayHtml(this HtmlHelper helper, string htmlText)
        {

            return new HtmlString( htmlText);
        }
    }
}