using System.Text.Json;

namespace SpyGame.Services
{
    public class PreferencesService
    {
        private static PreferencesService? _instance;
        private static readonly object _lock = new object();
        private readonly string _preferencesFileName = "preferences.json";

        public static PreferencesService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PreferencesService();
                        }
                    }
                }
                return _instance;
            }
        }

        private PreferencesService()
        {
        }

        public async Task SaveLastSelectedPack(string packName)
        {
            try
            {
                var preferences = await LoadPreferences();
                preferences.LastSelectedPack = packName;
                await SavePreferences(preferences);
            }
            catch
            {
            }
        }

        public async Task<string?> GetLastSelectedPack()
        {
            try
            {
                var preferences = await LoadPreferences();
                return preferences.LastSelectedPack;
            }
            catch
            {
                return null;
            }
        }

        private async Task<Preferences> LoadPreferences()
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _preferencesFileName);
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    return JsonSerializer.Deserialize<Preferences>(json) ?? new Preferences();
                }
            }
            catch
            {
            }
            return new Preferences();
        }

        private async Task SavePreferences(Preferences preferences)
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _preferencesFileName);
                var json = JsonSerializer.Serialize(preferences, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
            catch
            {
            }
        }

        private class Preferences
        {
            public string? LastSelectedPack { get; set; }
        }
    }
}
