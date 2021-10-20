using Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Helpers
{
    public static class HtmlHelpers
    {

        public static string IsSelected1(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }
        public static string IsSelected(this IHtmlHelper html, string cssClass, string area = null, string controller = null, string action = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];

            if (String.IsNullOrEmpty(area))
                area = currentArea;

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            List<string> x1 = new List<string>();
            if (String.IsNullOrEmpty(action))
                x1.Add(currentAction);

            if ( !string.IsNullOrEmpty(action) && action.Contains(","))
            {
                var x = action.Split(",").ToList();
                x1.AddRange(x);
            }
            else
            {
                x1.Add(action);
            }


            return area == currentArea && controller == currentController && x1.Contains(currentAction) ?
                cssClass : String.Empty;
        }
        public static string SelectCurrentController(this IHtmlHelper html, string cssClass, string area = null, string controllers = null)
        {
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];

            List<string> x1 = new List<string>();
            
            if (String.IsNullOrEmpty(controllers))
                x1.Add( currentController);
           
            if (!string.IsNullOrEmpty(controllers) && controllers.Contains(","))
            {
                var x = controllers.Split(",").ToList();
                x1.AddRange(x);
            }
            else
            {
                x1.Add(controllers);
            }

            if (String.IsNullOrEmpty(area))
                area = currentArea;


            return area == currentArea && x1.Contains(currentController) ? cssClass : "";
        }
        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }
        #region lấy cây, list
       
        #endregion
    }
}
