using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

using Shared;

using System;
using System.IO;

namespace Model.Extensions
{
    public static class MarkdownExtension
    {
        #region "Métodos"

        public static IHtmlContent RenderMarkdown(this IHtmlHelper helper, string filename)
        {
            var rootPath = (string)AppDomain.CurrentDomain.GetData("ContentRootPath");
            //var selectedLanguage = helper.ViewContext.Controller.ControllerContext.GetSelectedLanguage();

            //var pathSelected = helper.ViewContext.HttpContext.Server.MapPath(filename + "." + selectedLanguage + ".md");
            var pathDefault = Path.Combine(rootPath, filename.TrimStart('/').Replace("/", @"\\") + ".md");

            //if (!File.Exists(pathSelected))
            if (!File.Exists(pathDefault))
            {
                return helper.Raw("<div class=\"alert alert-danger fade in\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button><i class=\"icon-warning-sign\"></i> File " + pathDefault + " not found</div>");
            }
            //else
            //    pathSelected = pathDefault;

            //string cacheKey = pathSelected;
            string cacheKey = pathDefault;

            // Load source text
            var text = Cache.Get<String>(cacheKey, makeCopy: false);
            if (text == null) // || HttpContext.Current.IsDebuggingEnabled)
            {
                //text = File.ReadAllText(pathSelected);
                text = File.ReadAllText(pathDefault);
                Cache.Insert(cacheKey, text, Cache.DefaultExpirationTime, makeCopy: false);
            }

            // Setup processor
            var md = new MarkdownDeep.Markdown
            {
                SafeMode = false,
                ExtraMode = true,
                AutoHeadingIDs = true,
                MarkdownInHtml = true,
                NewWindowForExternalLinks = true
            };

            // Write it
            return helper.Raw(md.Transform(text));
        }

        #endregion

    }

}
