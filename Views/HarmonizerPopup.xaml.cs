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
        StatusLabel.Text = "Статус: Ждите...";
        await Task.Delay(300);

        if (TonicaPicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "Статус: Ожидает";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("Ошибка", "Пожалуйста, выберите тонику.", "Ок");
            return;
        }

        if (ModePicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "Статус: Ожидает";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("Ошибка", "Пожалуйста, выберите лад.", "Ок");
            return;
        }

        if (ChordsPerBarPicker.SelectedIndex == -1)
        {
            StatusLabel.Text = "Статус: Ожидает";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("Ошибка", "Пожалуйста, выберите количество аккордов на такт.", "Ок");
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
            StatusLabel.Text = "Статус: Ошибка, некорректная мелодия дорожки.";
            PopupGrid.IsEnabled = true;
            await Project.Playlist.DisplayAlert("Ошибка", "Была обнаружена некорректная расстановка нот. Ознакомьтесь с инструкцией по использованию гармонизатора и исправьте мелодию под требования его работы.", "Ок");
            return;
        }

        Track harmonyTrack = new Track($"{Track.Name}", "Piano");
        harmonyTrack.SetNotes(harmonyChords);
        Project.AddTrack(harmonyTrack);
        Project.Playlist.OnTrackAdded();
        StatusLabel.Text = "Статус: Дорожка с аккордами успешно добавлена!";

        PopupGrid.IsEnabled = true;
    }

    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }

    public async void HelpClicked(object sender, EventArgs e)
    {
        string title = "Как использовать Гармонизатор";
        string message = "Инструкция по использованию гармонизатора:\n\n1. Расставьте ноты мелодии какой-либо дорожки, причем ноты должны соответствовать нотам тональности.\n2. Добавьте в конце мелодии ноту, означающую разрешение мелодии.\n3. В окошке гармонзатора выберите тонику и лад и количество аккордов на такт. Учтите, что ноты мелодии должны соответствать выбранной вами тональности.\n4. Нажмите на кнопку гармонизации, убедитесь, что процесс выполнился без ошибок и окошко статуса подтверждает это.";
        string cancel = "Ок";

        await Project.Playlist.DisplayAlert(title, message, cancel);
    }
}