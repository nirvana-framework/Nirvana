namespace Nirvana.CQRS
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }
    }
}