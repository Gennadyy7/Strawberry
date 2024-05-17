using CommunityToolkit.Maui.Views;
using Strawberry.Entities;

namespace Strawberry.Views;

public partial class SettingsPopup : Popup
{
    public Project Project { get; set; }

    public event Action SettingsApplied;

    public SettingsPopup(Project project)
	{
		InitializeComponent();
        Project = project;
        ProjectNameEntry.Text = project.Name;
        ProjectBpmEntry.Text = project.Bpm.ToString();
    }

    public void TryingToChangeName(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ProjectNameEntry.Text) && ProjectNameEntry.Text.StartsWith(" "))
        {
            ProjectNameEntry.Text = ProjectNameEntry.Text.TrimStart(' ');
        }

        if (ProjectNameEntry.Text != null && ProjectNameEntry.Text.Length > 100)
        {
            string newText = ProjectNameEntry.Text.Substring(0, 100);
            ProjectNameEntry.Text = newText;
        }
    }

    public void TryingToChangeBpm(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ProjectBpmEntry.Text) && (ProjectBpmEntry.Text.StartsWith(" ") || ProjectBpmEntry.Text.StartsWith("-") || ProjectBpmEntry.Text.StartsWith("0")))
        {
            ProjectBpmEntry.Text = ProjectBpmEntry.Text.TrimStart(' ').TrimStart('-').TrimStart('0');
        }

        if (!string.IsNullOrEmpty(ProjectBpmEntry.Text) && (ProjectBpmEntry.Text.EndsWith(",") || ProjectBpmEntry.Text.EndsWith(".")))
        {
            ProjectBpmEntry.Text = ProjectBpmEntry.Text.TrimEnd(',').TrimEnd('.');
        }

        if (ProjectBpmEntry.Text != null && ProjectBpmEntry.Text.Length > 3)
        {
            string newText = ProjectBpmEntry.Text.Substring(0, 3);
            ProjectBpmEntry.Text = newText;
        }
    }

    public async void OkClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ProjectNameEntry.Text) || string.IsNullOrEmpty(ProjectBpmEntry.Text))
        {
            await Application.Current.MainPage.DisplayAlert("Заполните поля!", "Вы заполнили не все поля.", "OK");
        }
        else
        {
            Project.Name = ProjectNameEntry.Text;
            Project.Bpm = int.Parse(ProjectBpmEntry.Text);
            SettingsApplied?.Invoke();
            Close();
        }
    }

    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}