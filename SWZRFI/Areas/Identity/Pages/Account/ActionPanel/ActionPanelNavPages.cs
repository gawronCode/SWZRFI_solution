using Microsoft.AspNetCore.Mvc.Rendering;
using System;


namespace SWZRFI.Areas.Identity.Pages.Account.ActionPanel
{
    public static class ActionPanelNavPages
    {

        public static string Index => "Index";
        public static string IndexNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Index);


        public static string Messages => "Messages";
        public static string MessagesNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Messages);

        public static string Applications => "Applications";
        public static string ApplicationsNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Applications);

        public static string Cv => "Cv";
        public static string CvNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Cv);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
