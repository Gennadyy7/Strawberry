using CommunityToolkit.Maui.Views;
using Strawberry.Entities;
namespace Strawberry.Views;


public partial class TrackSettingsPopup : Popup
{
    public Track Track { get; set; }
    public static event Action TrackSettingsApplied;
    public TrackSettingsPopup(Track track)
    {
        InitializeComponent();
        Track = track;
        TrackNameEntry.Text = track.Name;
    }

    public void TrackNameChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(TrackNameEntry.Text) && TrackNameEntry.Text.StartsWith(" "))
        {
            TrackNameEntry.Text = TrackNameEntry.Text.TrimStart(' ');
        }

        if (TrackNameEntry.Text != null && TrackNameEntry.Text.Length > 16)
        {
            string newText = TrackNameEntry.Text.Substring(0, 16);
            TrackNameEntry.Text = newText;
        }
    }

    public async void OkClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(TrackNameEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("Заполните поле!", "Вы не заполнили поле.", "OK");
        }
        else
        {
            Track.SetName(TrackNameEntry.Text);
            TrackSettingsApplied?.Invoke();
            Close();
        }
    }
    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}