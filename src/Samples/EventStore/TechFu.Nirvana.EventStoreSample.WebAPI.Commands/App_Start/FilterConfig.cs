using System.Web.Mvc;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.Commands
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
