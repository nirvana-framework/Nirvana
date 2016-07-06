using System.Collections.Generic;

namespace TechFu.Nirvana.CQRS
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; }

        public int LastPage { get; set; }
        public int Total { get; set; }
        public int PerPage { get; set; }
        public int Page { get; set; }
    }
}