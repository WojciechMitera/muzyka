using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiApp4;

public partial class NewPage2 : ContentPage
{
    private ObservableCollection<Songs> _songs = new();
    private readonly string _songsFilePath = Path.Combine(FileSystem.AppDataDirectory, "songs.json");
    private int _currentIndex = -1;
    int quantity = 0;
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
        List<Songs> lista = [];

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
                for (int i = 0; i < _songs.Count; i++)
                {
                    lista.Add(_songs[i]);
                }
                ilosc.Text = lista.Count.ToString() + " songs";
            }
            await SaveSongsAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("B  d", $"Nie uda o si  wybra  plik w: {ex.Message}", "OK");
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
            Console.WriteLine($"B  d zapisu piosenek: {ex.Message}");
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
            Console.WriteLine($"B  d odczytu piosenek: {ex.Message}");
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

    private async void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Songs selected)
        {
            await Navigation.PushAsync(new MainPage(selected));
        }
        list.SelectedItem = null;
    }

    private void delete_Clicked(object sender, EventArgs e)
    {
        List<Songs> lista = [];
        var button = sender as Button;
        var s = button?.BindingContext as Songs;
        if (s != null)
        {
            _songs.Remove(s);
            for (int i = 0; i < _songs.Count; i++)
            {
                lista.Add(_songs[i]);
            }
            for (int i = 0; i < lista.Count; i++)
            {
                lista.Remove(s);
            }
            ilosc.Text = lista.Count.ToString() + " songs";
        }
    }
}