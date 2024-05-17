using CommunityToolkit.Maui.Views;
using Strawberry.Entities;

namespace Strawberry.Views;

public partial class AddTrackPopup : Popup
{
	public Project Project { get; set; }

    public event Action TrackAdded;
	public AddTrackPopup(Project project)
	{
		InitializeComponent();
		Project = project;
	}
	public void TryingToChangeText(object sender, EventArgs e)
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
        if (string.IsNullOrEmpty(TrackNameEntry.Text) || InstrumentPicker.SelectedItem == null)
        {
            await Application.Current.MainPage.DisplayAlert("Заполните поля!", "Вы заполнили не все поля.", "OK");
        }
        else
        {
            Project.AddTrack(TrackNameEntry.Text, InstrumentPicker.SelectedItem.ToString());
            TrackAdded?.Invoke();
            Close();
        }
    }
    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}