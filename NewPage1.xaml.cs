using System.Collections.ObjectModel;
using System.Text.Json;



namespace odtwarzacz_muzyki;

public partial class NewPage1 : ContentPage
{
    //private ObservableCollection<Songs> _songs = new();
    
    //private int _currentIndex = -1;
    ObservableCollection<Playlists> playlist { get; set; }
    public NewPage1()
    {
        InitializeComponent();
        playlist = new ObservableCollection<Playlists>();
        BindingContext = this;
    }



    

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Utwórz playliste", "");

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

}