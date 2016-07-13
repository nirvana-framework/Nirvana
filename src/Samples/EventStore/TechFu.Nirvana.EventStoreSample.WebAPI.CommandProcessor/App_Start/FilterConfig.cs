using System.Web.Mvc;

namespace TechFu.Nirvana.EventStoreSample.WebAPI.CommandProcessor
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
