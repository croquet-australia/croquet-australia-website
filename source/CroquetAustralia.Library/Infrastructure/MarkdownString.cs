namespace CroquetAustralia.Library.Infrastructure
{
    public class MarkdownString : IMarkdownString
    {
        private readonly string _value;

        public MarkdownString(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
