using Microsoft.AspNetCore.Mvc.Rendering;
using System;


namespace SWZRFI.Areas.Identity.Pages.Account.ConversationPanel
{
    public static class ConversationPanelNavPages
    {

        public static string Index => "Index";
        public static string IndexNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Index);

        public static string Recruiters => "Recruiters";
        public static string RecruitersNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Recruiters);

        public static string Candidates => "Candidates";
        public static string CandidatesNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Candidates);

        public static string Employees => "Employees";
        public static string EmployeesNavClass(ViewContext viewContext)
            => PageNavClass(viewContext, Employees);


        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
