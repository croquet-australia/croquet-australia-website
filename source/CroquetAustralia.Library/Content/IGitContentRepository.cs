using Casper.Domain.Features.Authors;

namespace CroquetAustralia.Library.Content
{
    public interface IGitContentRepository
    {
        string Directory { get; }

        void Start();
        void CommitAndPush(string relativePath, Author author);
    }
}