using CommunityToolkit.Maui.Views;
using Strawberry.Entities;

namespace Strawberry.Views;

public partial class TrackSlidersPopup : Popup
{
    public Track Track { get; set; }
    public static event Action TrackSlidersApplied;
    public TrackSlidersPopup(Track track)
	{
		InitializeComponent();
        Track = track;
        VolumeEntry.Text = Track.Volume.ToString();
        BalanceEntry.Text = Track.Pan.ToString();
	}

    public void OkClicked(object sender, EventArgs e)
	{
        if (int.TryParse(VolumeEntry.Text, out int volumeValue))
        {
            Track.SetVolume(volumeValue);
        }
        if (int.TryParse(BalanceEntry.Text, out int balanceValue))
        {
            Track.SetPan(balanceValue);
        }
        TrackSlidersApplied?.Invoke();
        Close();
    }

    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    public void VolumeChanged(object sender, TextChangedEventArgs e)
    {
        if (!int.TryParse(VolumeEntry.Text, out int volume))
        {
            VolumeEntry.Text = string.Empty;
        }
        else
        {
            volume = Math.Max(0, Math.Min(100, volume));
            VolumeEntry.Text = volume.ToString();
        }
    }

    public void BalanceChanged(object sender, TextChangedEventArgs e)
    {
        if (BalanceEntry.Text == "-")
        {
            return;
        }

        if (!int.TryParse(BalanceEntry.Text, out int balance))
        {
            BalanceEntry.Text = string.Empty;
        }
        else
        {
            balance = Math.Max(-100, Math.Min(100, balance));
            BalanceEntry.Text = balance.ToString();
        }
    }
}