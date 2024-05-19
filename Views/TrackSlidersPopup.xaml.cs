using CommunityToolkit.Maui.Views;

namespace Strawberry.Views;

public partial class TrackSlidersPopup : Popup
{
	public TrackSlidersPopup()
	{
		InitializeComponent();
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