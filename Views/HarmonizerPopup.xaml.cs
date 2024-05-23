using CommunityToolkit.Maui.Views;
using Strawberry.Entities;

namespace Strawberry.Views;

public partial class HarmonizerPopup : Popup
{
    public Project Project { get; set; }
    public Track Track { get; set; }

	public HarmonizerPopup(Project project, Track track)
	{
		InitializeComponent();
        Project = project;
        Track = track;
	}

    public async void HarmonizeClicked(object sender, EventArgs e)
    {
        PopupGrid.IsEnabled = false;
        StatusLabel.Text = "������: �����...";
        await Task.Delay(300);

        if (TonicaPicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "������: �������";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("������", "����������, �������� ������.", "��");
            return;
        }

        if (ModePicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "������: �������";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("������", "����������, �������� ���.", "��");
            return;
        }

        if (ChordsPerBarPicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "������: �������";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("������", "����������, �������� ���������� �������� �� ����.", "��");
            return;
        }

        string selectedTonica = TonicaPicker.SelectedItem.ToString();
        Pitch pitch = (Pitch)Harmonizer.GetPitchFromTonica(selectedTonica);

        string selectedMode = ModePicker.SelectedItem.ToString();
        HarmonyMode mode = (HarmonyMode)Enum.Parse(typeof(HarmonyMode), selectedMode);

        int chordsNumber = Int32.Parse(ChordsPerBarPicker.SelectedItem.ToString());

        var harmonyChords = Harmonizer.Harmonize(Track, pitch, mode, chordsNumber);

        if (harmonyChords == null)
        {
            StatusLabel.Text = "������: ������, ������������ ������� �������.";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("������", "���� ���������� ������������ ����������� ���. ������������ � ����������� �� ������������� ������������� � ��������� ������� ��� ���������� ��� ������.", "��");
            return;
        }

        Track harmonyTrack = new Track($"{Track.Name}", "Piano");
        harmonyTrack.SetNotes(harmonyChords);
        Project.AddTrack(harmonyTrack);
        Project.Playlist.OnTrackAdded();
        StatusLabel.Text = "������: ������� � ��������� ������� ���������!";

        PopupGrid.IsEnabled = true;
    }

    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    public async void HelpClicked(object sender, EventArgs e)
    {
        string title = "��� ������������ ������������";
        string message = "���������� �� ������������� �������������:\n\n1. ���������� ���� ������� �����-���� �������, ������ ���� ������ ��������������� ����� �����������.\n2. �������� � ����� ������� ����, ���������� ���������� �������.\n3. � ������ ������������ �������� ������ � ��� � ���������� �������� �� ����. ������, ��� ���� ������� ������ ������������� ��������� ���� �����������.\n4. ������� �� ������ ������������, ���������, ��� ������� ���������� ��� ������ � ������ ������� ������������ ���.";
        string cancel = "��";

        await Project.Playlist.DisplayAlert(title, message, cancel);
    }
}