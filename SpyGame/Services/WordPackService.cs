using SpyGame.Models;

namespace SpyGame.Services
{
    public class WordPackService
    {
        private List<WordPack> _wordPacks = new();

        public WordPackService()
        {
            InitializeDefaultPacks();
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

        public List<WordPack> GetAllPacks()
        {
            return _wordPacks;
        }

        public WordPack? GetPackByName(string name)
        {
            return _wordPacks.FirstOrDefault(pack => pack.Name == name);
        }
    }
}
