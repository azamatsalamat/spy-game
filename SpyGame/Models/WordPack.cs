namespace SpyGame.Models
{
    public class WordPack
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Words { get; set; } = new List<string>();

        public WordPack()
        {
        }

        public WordPack(string name, List<string> words)
        {
            Name = name;
            Words = words;
        }
    }
}
