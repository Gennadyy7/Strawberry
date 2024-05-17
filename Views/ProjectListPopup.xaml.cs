using CommunityToolkit.Maui.Views;

namespace Strawberry.Views;

public partial class ProjectListPopup : Popup
{
    public event Action<ProjectListPopup> ProjectSelected;

    public string ProjectName { get; set; }
    public ProjectListPopup()
	{
		InitializeComponent();

        using (var db = new ApplicationContext())
        {
            var projectNames = db.Projects.Select(p => p.Name).ToList();

            foreach (var projectName in projectNames)
            {
                ProjectPicker.Items.Add(projectName);
            }
        }
    }

    public async void OkClicked(object sender, EventArgs e)
    {
        if (ProjectPicker.SelectedItem == null)
        {
            await Application.Current.MainPage.DisplayAlert("���������� ��������� ������!", "���������� ��������� ������, ��� ��� �� ���� ������� ��� ��������.", "OK");
        }
        else
        {
            ProjectName = ProjectPicker.SelectedItem.ToString();
            ProjectSelected?.Invoke(this);
            Close();
        }
    }

    public void CancelClicked(object sender, EventArgs e)
    {
        Close();
    }
}