namespace CroquetAustraliaWebsite.Library.Content
{
    public interface IPagination
    {
        int PageNumber { get; }
        int PageSize { get; }
    }
}