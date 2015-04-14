namespace CroquetAustraliaWebsite.Library.Content
{
    public class Pagination : IPagination
    {
        public Pagination()
            : this(1)
        {
        }

        public Pagination(int pageNumber)
            : this(pageNumber, 10)
        {
        }

        public Pagination(int pageNumber, int pageSize)
        {
            // todo: argument validation

            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
    }
}