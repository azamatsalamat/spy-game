using SpyGame.Models;
using System.Text.Json;

namespace SpyGame.Services
{
    public class WordPackService
    {
        private static WordPackService? _instance;
        private static readonly object _lock = new object();
        
        private List<WordPack> _wordPacks = new();
        private List<WordPack> _customPacks = new();
        private readonly string _customPacksFileName = "custom_packs.json";

        public static WordPackService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new WordPackService();
                        }
                    }
                }
                return _instance;
            }
        }

        private WordPackService()
        {
            InitializeDefaultPacks();
            _ = LoadCustomPacksAsync();
        }

        private void InitializeDefaultPacks()
        {
            _wordPacks = new List<WordPack>
            {
                new WordPack("Famous Actors", new List<string>
                {
                    "Tom Hanks", "Brad Pitt", "Leonardo DiCaprio", "Meryl Streep", "Julia Roberts",
                    "Denzel Washington", "Sandra Bullock", "Johnny Depp", "Angelina Jolie", "Will Smith",
                    "Nicole Kidman", "George Clooney", "Charlize Theron", "Matt Damon", "Cate Blanchett"
                }),

                new WordPack("TV Shows", new List<string>
                {
                    "Friends", "Breaking Bad", "Game of Thrones", "The Office", "Stranger Things",
                    "The Walking Dead", "Modern Family", "Grey's Anatomy", "The Big Bang Theory", "Lost",
                    "House of Cards", "Orange is the New Black", "The Crown", "Westworld", "Black Mirror"
                }),

                new WordPack("Movies", new List<string>
                {
                    "Titanic", "Avatar", "The Avengers", "Jurassic Park", "Forrest Gump",
                    "The Lion King", "Star Wars", "The Matrix", "Inception", "The Dark Knight",
                    "Pulp Fiction", "Fight Club", "The Godfather", "Casablanca", "Gone with the Wind"
                }),

                new WordPack("Food & Drinks", new List<string>
                {
                    "Pizza", "Sushi", "Burger", "Pasta", "Steak",
                    "Ice Cream", "Coffee", "Wine", "Chocolate", "Salad",
                    "Soup", "Sandwich", "Cake", "Beer", "Tea"
                }),

                new WordPack("Animals", new List<string>
                {
                    "Lion", "Elephant", "Giraffe", "Penguin", "Dolphin",
                    "Eagle", "Shark", "Tiger", "Bear", "Wolf",
                    "Monkey", "Zebra", "Kangaroo", "Panda", "Koala"
                }),

                new WordPack("Countries", new List<string>
                {
                    "United States", "Canada", "United Kingdom", "France", "Germany",
                    "Japan", "Australia", "Brazil", "India", "China",
                    "Italy", "Spain", "Mexico", "Russia", "South Africa"
                }),

                new WordPack("Sports", new List<string>
                {
                    "Football", "Basketball", "Baseball", "Soccer", "Tennis",
                    "Golf", "Swimming", "Volleyball", "Hockey", "Cricket",
                    "Rugby", "Boxing", "Wrestling", "Gymnastics", "Athletics"
                }),

                new WordPack("Jobs", new List<string>
                {
                    "Doctor", "Teacher", "Engineer", "Chef", "Police Officer",
                    "Firefighter", "Lawyer", "Artist", "Musician", "Writer",
                    "Actor", "Pilot", "Nurse", "Architect", "Scientist"
                })
            };
        }

        public WordPack? GetPackByName(string name)
        {
            var defaultPack = _wordPacks.FirstOrDefault(pack => pack.Name == name);
            if (defaultPack != null) return defaultPack;
            
            return _customPacks.FirstOrDefault(pack => pack.Name == name);
        }

        public List<WordPack> GetAllPacks()
        {
            var allPacks = new List<WordPack>();
            allPacks.AddRange(_customPacks);
            allPacks.AddRange(_wordPacks);
            return allPacks;
        }

        public List<WordPack> GetDefaultPacks()
        {
            return _wordPacks;
        }

        public List<WordPack> GetCustomPacks()
        {
            return _customPacks;
        }

        public async Task AddCustomPack(WordPack pack)
        {
            if (string.IsNullOrWhiteSpace(pack.Name))
                throw new ArgumentException("Pack name cannot be empty");

            if (pack.Words.Count < 2)
                throw new ArgumentException("Pack must contain at least 2 words");

            if (_wordPacks.Any(p => p.Name == pack.Name) || _customPacks.Any(p => p.Name == pack.Name))
                throw new ArgumentException("A pack with this name already exists");

            pack.IsCustom = true;
            _customPacks.Add(pack);
            await SaveCustomPacks();
        }

        public async Task DeleteCustomPack(string packName)
        {
            var pack = _customPacks.FirstOrDefault(p => p.Name == packName);
            if (pack != null)
            {
                _customPacks.Remove(pack);
                await SaveCustomPacks();
            }
        }

        public async Task UpdateCustomPack(string oldName, string newName, List<string> words)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Pack name cannot be empty");

            if (words.Count < 2)
                throw new ArgumentException("Pack must contain at least 2 words");

            var existingPack = _customPacks.FirstOrDefault(p => p.Name == oldName);
            if (existingPack == null)
                throw new ArgumentException("Pack not found");

            if (oldName != newName && (_wordPacks.Any(p => p.Name == newName) || _customPacks.Any(p => p.Name == newName)))
                throw new ArgumentException("A pack with this name already exists");

            existingPack.Name = newName;
            existingPack.Words = words;
            await SaveCustomPacks();
        }

        public async Task RefreshCustomPacks()
        {
            await LoadCustomPacksAsync();
        }

        private async Task LoadCustomPacksAsync()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _customPacksFileName);
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    _customPacks = JsonSerializer.Deserialize<List<WordPack>>(json) ?? new List<WordPack>();
                }
            }
            catch
            {
                _customPacks = new List<WordPack>();
            }
        }

        private async Task SaveCustomPacks()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _customPacksFileName);
                var json = JsonSerializer.Serialize(_customPacks, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
            catch
            {
                // Handle save error silently for now
            }
        }
    }
}
