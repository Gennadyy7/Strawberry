using CommunityToolkit.Maui.Views;
using Strawberry.Entities;

namespace Strawberry.Views;

public partial class TrackSlidersPopup : Popup
{
    public Track Track { get; set; }
	public TrackSlidersPopup(Track track)
	{
		InitializeComponent();
        Track = track;
        VolumeEntry.Text = Track.Volume.ToString();
        BalanceEntry.Text = Track.Pan.ToString();
	}

    public void OkClicked(object sender, EventArgs e)
	{
        Close();
    }

    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}