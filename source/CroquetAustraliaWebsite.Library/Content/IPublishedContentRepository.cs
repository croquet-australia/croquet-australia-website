namespace CroquetAustraliaWebsite.Library.Content
{
    public interface IPublishedContentRepository
    {
        void Start();
        void Publish(string relativePath);
    }
}