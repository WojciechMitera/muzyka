using Plugin.Maui.Audio;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace odtwarzacz_muzyki
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Songs> _songs = new();

        private readonly string _songsFilePath = Path.Combine(FileSystem.AppDataDirectory, "songs.json");

        private IAudioPlayer _player; // Korzystamy teraz z doinstalowanej biblioteki Plugin.Maui.Audio
        private readonly IAudioManager _audioManager; // /\
        private int _currentSongIndex = -1;


        Songs _tytul;
        public MainPage(Songs selected)
        {
            InitializeComponent();
            _tytul = selected;
            tytul.Text = _tytul.Title;
            _audioManager = AudioManager.Current; // tworzymy "Singleton" -- ciekawy wzozerz projektowy który umożliwia tworzenie tylko jednej instancji danej klasy na cała aplikacje
            _ = LoadSongsAsync();
        }

        private async Task SaveSongsAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(_songs);
                await File.WriteAllTextAsync(_songsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisu piosenek: {ex.Message}");
            }
        }

        private async Task LoadSongsAsync()
        {
            try
            {
                if (File.Exists(_songsFilePath))
                {
                    var json = await File.ReadAllTextAsync(_songsFilePath);
                    var loadedSongs = JsonSerializer.Deserialize<ObservableCollection<Songs>>(json);

                    if (loadedSongs != null)
                    {
                        _songs.Clear();
                        foreach (var song in loadedSongs)
                            _songs.Add(song);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd odczytu piosenek: {ex.Message}");
            }
        }

        private async void PrevButton_Clicked(object sender, EventArgs e)
        {
            if (_songs.Count == 0) return;

            _currentSongIndex = (_currentSongIndex - 1 + _songs.Count) % _songs.Count;
            tytul.Text = _songs[_currentSongIndex].Title;
            await PlaySong(_songs[_currentSongIndex]);
        }

        private async void PlayButton_Clicked(object sender, EventArgs e)
        {
            // Ustawiamy indeks na pierwszą piosenke
            if (_currentSongIndex == -1 && _songs.Count > 0)
                _currentSongIndex = 0;

            // Sprawdzamy poprawność ideksu
            if (_currentSongIndex >= 0 && _currentSongIndex < _songs.Count)
            {
                // Odtwarzamy piosenke przy pomocy napisanej funkcji PlaySong przekazując ją z odpowiednim indeksem
                await PlaySong(_songs[_currentSongIndex]);
            }
        }

        private async Task PlaySong(Songs song)
        {
            try
            {
                // To powodowało problemy w poprzedniej próbie, musimy zatrzymać i usunąć odtwarzacz jeżeli juz istnieje
                _player?.Stop();
                _player?.Dispose();


                //  Otwieramy piosenkę
                using var stream = File.OpenRead(song.Path);

                // Tworzymy nowy odtwarzacz
                _player = _audioManager.CreatePlayer(stream);

                // I powinno być słychać jakieś dzwięki :D 
                _player.Play();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie można odtworzyć piosenki: {ex.Message}", "OK");
            }
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (_songs.Count == 0) return;
            
            _currentSongIndex = (_currentSongIndex + 1) % _songs.Count;
            tytul.Text = _songs[_currentSongIndex].Title;
            await PlaySong(_songs[_currentSongIndex]);
        }

        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            if (_player == null)
                return;

            if (_player.IsPlaying)
                _player.Pause();
            else
                _player.Play();
        }

    }

}
