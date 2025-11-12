using System.Collections.ObjectModel;



namespace odtwarzacz_muzyki;

public partial class NewPage1 : ContentPage
{
    //private ObservableCollection<Songs> _songs = new();
    //private readonly string _songsFilePath = Path.Combine(FileSystem.AppDataDirectory, "songs.json");
    //private int _currentIndex = -1;
    ObservableCollection<Playlists> playlist { get; set; }
    public NewPage1()
    {
        InitializeComponent();
        playlist = new ObservableCollection<Playlists>();
        BindingContext = this;
    }



    /*private async Task SaveSongsAsync()
    {
        try
        {
            var json = JsonSerializer.Serialize(_songs);
            await File.WriteAllTextAsync(_songsFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"B³¹d zapisu piosenek: {ex.Message}");
        }
    }*/



    private async void Button_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Utwórz playliste","");
        
        playlist.Add(new Playlists { Title = result });
        list.ItemsSource = playlist;
    }

    private async void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Playlists selected)
        {
            await Navigation.PushAsync(new NewPage2(selected));
        }
        list.SelectedItem = null;

    }

    private void delete_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var _playlist = button?.BindingContext as Playlists;
        if (_playlist != null)
        {
            playlist.Remove(_playlist);
        }
    }


    /*{
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
       }
       await SaveSongsAsync();
   }
   catch (Exception ex)
   {
       await DisplayAlert("B³¹d", $"Nie uda³o siê wybraæ plików: {ex.Message}", "OK");
   }*/
}