namespace SpyGame.Models
{
    public class WordPack
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Words { get; set; } = new List<string>();
        public bool IsCustom { get; set; } = false;
        public bool IsLastSelected { get; set; } = false;

        public WordPack()
        {
        }

        public WordPack(string name, List<string> words)
        {
            Name = name;
            Words = words;
            IsCustom = false;
        }

        public WordPack(string name, List<string> words, bool isCustom)
        {
            Name = name;
            Words = words;
            IsCustom = isCustom;
        }
    }
}
