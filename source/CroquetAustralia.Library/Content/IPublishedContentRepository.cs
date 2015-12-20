namespace CroquetAustralia.Library.Content
{
    public interface IPublishedContentRepository
    {
        void Start();
        void Publish(string relativePath);
    }
}