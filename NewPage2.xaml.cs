using System.Collections.ObjectModel;
using System.Text.Json;

namespace odtwarzacz_muzyki;

public partial class NewPage2 : ContentPage
{
    private ObservableCollection<Songs> _songs = new();
    private readonly string _songsFilePath = Path.Combine(FileSystem.AppDataDirectory, "songs.json");
    private int _currentIndex = -1;
    //int quantity = 0;
    Playlists _playlist;
    public NewPage2(Playlists selected)
    {
        InitializeComponent();
        _playlist = selected;
        tytul.Text = _playlist.Title;
        list.ItemsSource = _songs;
    }
    private async void Add_Button_Clicked(object sender, EventArgs e)
    {
        //ilosc.Text = quantity + " songs";
        try
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Wybierz pliki muzyczne",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".mp3", ".wav", ".flac" } },
                        { DevicePlatform.Android, new[] { "audio/*" } },
                        { DevicePlatform.iOS, new[] { "public.audio" } }
                    })
            });

            if (result != null)
            {
                foreach (var file in result)
                {
                    _songs.Add(new Songs
                    {
                        Title = System.IO.Path.GetFileNameWithoutExtension(file.FileName),
                        Path = file.FullPath
                    });

                }
                //quantity++;
            }
            await SaveSongsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", $"Nie udało się wybrać plików: {ex.Message}", "OK");
        }
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


    private void NextTrack()
    {

    }

    private void PrevButton_Clicked(object sender, EventArgs e)
    {
    }

    private void Add_Button_Clicked_1(object sender, EventArgs e)
    {

    }

    /*private void PlayButton_Clicked(object sender, EventArgs e)
    {
        if (_songs.Count == 0) return;
        if (_currentIndex == -1) _currentIndex = 0;

        Player.Source = _songs[_currentIndex].Path; // ustaw Source zawsze
        Player.Play();
        CurrentSongLabel.Text = $"▶ {_songs[_currentIndex].Title}";
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {

    }

    private void PauseButton_Clicked(object sender, EventArgs e)
    {
        if (Player.CurrentState == MediaElementState.Playing)
        {
            Player.Pause();
            CurrentSongLabel.Text = $"⏸ {_songs[_currentIndex].Title}";
        }
        else if (Player.CurrentState == MediaElementState.Paused)
        {
            Player.Play();
            CurrentSongLabel.Text = $"▶ {_songs[_currentIndex].Title}";
        }
        else
        {
            DisplayAlert("Info", "Brak odtwarzania do wznowienia.", "OK");
        }
    }*/
}