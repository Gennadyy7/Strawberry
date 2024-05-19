using CommunityToolkit.Maui.Views;
using Microsoft.EntityFrameworkCore;
using Strawberry.Entities;
using Strawberry.Models;
using Strawberry.Views;

namespace Strawberry;

public partial class Playlist : ContentPage
{
    public Project Project { get; set; }
    private bool isDeleteMode = false;
    public static event Action<bool> OptimizationEvent;

    public Playlist()
	{
		InitializeComponent();
        StopButt.IsEnabled = false;

        Pianoroll.BackEvent += BackHandler;

        TrackLabelsScrollView.Scrolled += (s, e) =>
        {
            TrackScrollView.ScrollToAsync(0, TrackLabelsScrollView.ScrollY, true);
        };

        /*LineScrollView.Scrolled += (s, e) =>
        {
            TrackScrollView.ScrollToAsync(LineScrollView.ScrollX, 0, true);
        };*/

        Project = new Project("Test2", 120);
        ProjNameLabel.Text = $"Project: {Project.Name}";
        ProjBpmLabel.Text = $"Bpm: {Project.Bpm}";

        for (int col = 0; col < 16; col++)
        {
            Line.ColumnDefinitions.Add(new ColumnDefinition(25));
            Numbers.ColumnDefinitions.Add(new ColumnDefinition(25));

            Frame frame = new Frame
            {
                WidthRequest = 25,
                HeightRequest = 25,
                CornerRadius = 1,
                BorderColor = Colors.Black,
                BackgroundColor = Colors.White,
                VerticalOptions = LayoutOptions.End
            };

            Label numberLabel = new Label
            {
                WidthRequest = 25,
                HeightRequest = 17,
                Text = (col + 1).ToString(),
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                TextColor = Colors.White
            };

            if (col % 4 == 0)
            {
                numberLabel.FontAttributes = FontAttributes.Bold;
            }

            Numbers.Add(numberLabel, column: col);
            Line.Add(frame, column: col);
        }

        using (var db = new ApplicationContext())
        {
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        Project.Playlist = this;
    }

    public async void PlaylistUpdate()
    {
        await ClearLine();
        await LineUpdate();
        await ClearTrackGrid();
        await TrackGridUpdate();
    }

    public async Task LineUpdate()
    {
        for (int col = 0; col < Project.Length; col++)
        {
            Line.ColumnDefinitions.Add(new ColumnDefinition(25));
            Numbers.ColumnDefinitions.Add(new ColumnDefinition(25));

            Frame frame = new Frame
            {
                WidthRequest = 25,
                HeightRequest = 25,
                CornerRadius = 1,
                BorderColor = Colors.Black,
                BackgroundColor = Colors.White,
                VerticalOptions = LayoutOptions.End
            };

            Label numberLabel = new Label
            {
                WidthRequest = 25,
                HeightRequest = 17,
                Text = (col + 1).ToString(),
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.End,
                TextColor = Colors.White
            };

            if (col % 4 == 0)
            {
                numberLabel.FontAttributes = FontAttributes.Bold;
            }

            Numbers.Add(numberLabel, column: col);
            Line.Add(frame, column: col);

            await Task.Delay(0);
        }
    }

    public async Task ClearLine()
    {
        Line.Children.Clear();
        Line.RowDefinitions.Clear();
        Line.ColumnDefinitions.Clear();

        await Task.Delay(0);
    }

    public async Task ClearTrackGrid()
    {
        TrackGrid.Children.Clear();
        TrackGrid.RowDefinitions.Clear();
        TrackGrid.ColumnDefinitions.Clear();

        await Task.Delay(0);
    }

    public async Task TrackGridUpdate()
    {
        foreach (var track in Project.Tracks)
        {
            int trackIndex = Project.Tracks.IndexOf(track);
            var trackGrid = (Grid)TrackLabelsGrid.Children[trackIndex];
            var trackLabel = (Label)trackGrid.Children.FirstOrDefault(x => x.GetType() == typeof(Label));
            var button = new Grid
            {
                WidthRequest = 25 * Project.Length,
                HeightRequest = 52,
                BackgroundColor = trackLabel.BackgroundColor,
                Children =
                {
                    new Label
                    {
                        Text = track.Name + " (кол-во нот: " + track.Notes.Sum(n => n.Value.Count) + ")",
                        TextColor = Colors.Black,
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(5, 0, 0, 0)
                    }
                }
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += TrackPressed;
            button.GestureRecognizers.Add(tapGestureRecognizer);
            TrackGrid.AddRowDefinition(new RowDefinition(52));
            TrackGrid.Children.Add(button);
            TrackGrid.SetRow(button, Project.Tracks.IndexOf(track));

            await Task.Delay(0);
        }
    }

    public void ToPosition(int position)
    {
        Application.Current.Dispatcher.Dispatch(() =>
        {
            Line.Children.Cast<Frame>().First(c => Grid.GetColumn(c) == position).BackgroundColor = Colors.Red;
            if (position != 0)
            {
                Line.Children.Cast<Frame>().First(c => Grid.GetColumn(c) == position - 1).BackgroundColor = Colors.White;
            }
        });
    }

    public void AddClicked(object sender, EventArgs e)
    {
        var popup = new AddTrackPopup(Project);
        popup.TrackAdded += OnTrackAdded;
        this.ShowPopup(popup);
    }

    public async void BackHandler()
    {
        var page = Content;
        Content = new Label
        {
            Text = "Загрузка...",
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.DarkSlateGray,
            TextColor = Colors.White,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
        };
        await Task.Delay(300);

        int length = 0;
        try
        {
            length = (int)((Math.Floor(Project.Tracks.SelectMany(track => track.Notes.Keys).Max() / 16.0) + 1) * 16);
        }
        catch
        {
            length = 0;
        }
        Project.Length = length != 0 ? length : 16;

        PlaylistUpdate();

        Content = page;
    }

    private void OnTrackAdded()
    {
        Random random = new Random();
        var button = new Grid
        {
            WidthRequest = 25 * Project.Length,
            HeightRequest = 52,
            BackgroundColor = Color.FromRgb(random.Next(130, 170), random.Next(130, 170), random.Next(130, 170)),
            Children =
            {
                new Label
                {
                    Text = Project.Tracks.Last().Name + " (кол-во нот: " + Project.Tracks.Last().Notes.Sum(n => n.Value.Count) + ")",
                    TextColor = Colors.Black,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(5, 0, 0, 0)
                }
            }
        };

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += TrackPressed;
        button.GestureRecognizers.Add(tapGestureRecognizer);
        TrackGrid.AddRowDefinition(new RowDefinition(52));
        TrackGrid.Children.Add(button);
        TrackGrid.SetRow(button, Project.Tracks.Count - 1);


        var track = Project.Tracks.Last();
        var VolStr = track.IsMuted ? "Заглушен" : $"{track.Volume}% громкости";
        var trackLabel = new Label
        {
            Text = $"Имя: {track.Name}\n{VolStr}",
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Start,
            HeightRequest = 52,
            TextColor = Colors.Black,
            BackgroundColor = button.BackgroundColor,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(5, 0, 0, 0)
        };

        var instrumentImage = new Image
        {
            Source = $"{track.Instrument.ToLower()}.png",
            HeightRequest = 50,
            WidthRequest = 50,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center
        };

        var grid = new Grid
        {
            BackgroundColor = button.BackgroundColor,
            Children = { trackLabel, instrumentImage },
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
        };

        TrackLabelsGrid.AddRowDefinition(new RowDefinition(52));
        TrackLabelsGrid.Children.Add(grid);
        TrackLabelsGrid.SetRow(grid, Project.Tracks.Count - 1);
    }

    private async void TrackPressed(object sender, EventArgs e)
    {
        if (isDeleteMode)
        {
            var confirmed = await DisplayAlert("Подтверждение", "Вы уверены, что хотите удалить дорожку?", "Да", "Нет");
            if (!confirmed)
            {
                return;
            }

            var trackButton = (Grid)sender;
            var trackIndex = TrackGrid.GetRow(trackButton);
            Project.RemoveTrack(Project.Tracks[trackIndex]);
            TrackGrid.Children.Remove(trackButton);
            TrackGrid.RowDefinitions.RemoveAt(trackIndex);

            var trackLabel = TrackLabelsGrid.Children[trackIndex];
            TrackLabelsGrid.Children.Remove(trackLabel);
            TrackLabelsGrid.RowDefinitions.RemoveAt(trackIndex);

            for (int i = trackIndex; i < TrackGrid.Children.Count; i++)
            {
                TrackGrid.SetRow(TrackGrid.Children[i], i);
                TrackLabelsGrid.SetRow(TrackLabelsGrid.Children[i], i);
            }
        }
        else
        {
            StopClicked(sender, e);
            var trackButton = (Grid)sender;
            var trackIndex = TrackGrid.GetRow(trackButton);
            var selectedTrack = Project.Tracks[trackIndex];
            SwitchToPianoroll(trackIndex);
        }
    }

    public async void SwitchToPianoroll(int trackIndex)
    {
        await Navigation.PushAsync(new Pianoroll(Project, trackIndex), false);
    }

    public void RemClicked(object sender, EventArgs e)
    {
        isDeleteMode = !isDeleteMode;
        RemButt.Background = isDeleteMode ? Colors.DarkOrange : Colors.DarkRed;
    }

    public async void SaveClicked(object sender, EventArgs e)
    {
        using (var db = new ApplicationContext())
        {
            var existingProject = db.Projects
            .Include(p => p.Tracks)
            .ThenInclude(t => t.Notes)
            .FirstOrDefault(p => p.Name == Project.Name);

            if (existingProject != null)
            {
                var confirmed = await DisplayAlert("Подтверждение", "Проект с таким именем уже существует. Вы уверены, что хотите перезаписать его?", "Да", "Нет");
                if (!confirmed)
                {
                    return;
                }

                existingProject.Bpm = Project.Bpm;
                db.Tracks.RemoveRange(existingProject.Tracks);
                existingProject.Tracks = Project.Tracks.Select(track => new TrackModel
                {
                    Name = track.Name,
                    Instrument = track.Instrument,
                    Volume = track.Volume,
                    IsMuted = track.IsMuted.ToString(),
                    Pan = track.Pan,
                    Notes = track.Notes.SelectMany(kvp => kvp.Value.Select(note => new NoteModel
                    {
                        NotePitch = note.NotePitch.ToString(),
                        Duration = note.Duration,
                        Position = note.Position,
                        Volume = note.Volume,
                        EffectiveVolume = note.EffectiveVolume,
                        Pan = note.Pan,
                        EffectivePan = note.EffectivePan
                    })).ToList()
                }).ToList();
            }
            else
            {
                var projectModel = new ProjectModel
                {
                    Name = Project.Name,
                    Bpm = Project.Bpm,
                    Tracks = Project.Tracks.Select(track => new TrackModel
                    {
                        Name = track.Name,
                        Instrument = track.Instrument,
                        Volume = track.Volume,
                        IsMuted = track.IsMuted.ToString(),
                        Pan = track.Pan,
                        Notes = track.Notes.SelectMany(kvp => kvp.Value.Select(note => new NoteModel
                        {
                            NotePitch = note.NotePitch.ToString(),
                            Duration = note.Duration,
                            Position = note.Position,
                            Volume = note.Volume,
                            EffectiveVolume = note.EffectiveVolume,
                            Pan = note.Pan,
                            EffectivePan = note.EffectivePan
                        })).ToList()
                    }).ToList()
                };

                db.Projects.Add(projectModel);
            }
            
            db.SaveChanges();
        }
    }
    public void DownClicked(object sender, EventArgs e)
    {
        var popup = new ProjectListPopup();
        popup.ProjectSelected += OnProjectSelected;
        this.ShowPopup(popup);
    }

    private void OnProjectSelected(ProjectListPopup popup)
    {
        using (var db = new ApplicationContext())
        {

            var projectModel = db.Projects
                .Include(p => p.Tracks)
                .ThenInclude(t => t.Notes)
                .FirstOrDefault(p => p.Name == popup.ProjectName);

            if (projectModel == null) return;

            Project = new Project
            {
                Name = projectModel.Name,
                Bpm = projectModel.Bpm,
                Tracks = projectModel.Tracks.Select(trackModel => new Track
                {
                    Name = trackModel.Name,
                    Instrument = trackModel.Instrument,
                    Volume = trackModel.Volume,
                    IsMuted = Convert.ToBoolean(trackModel.IsMuted),
                    Pan = trackModel.Pan,
                    Notes = trackModel.Notes.GroupBy(noteModel => noteModel.Position)
                        .ToDictionary(g => g.Key, g => g.Select(noteModel => new Note
                        {
                            NotePitch = (Pitch)Enum.Parse(typeof(Pitch), noteModel.NotePitch, true),
                            Duration = noteModel.Duration,
                            Position = noteModel.Position,
                            Volume = noteModel.Volume,
                            EffectiveVolume = noteModel.EffectiveVolume,
                            Pan = noteModel.Pan,
                            EffectivePan = noteModel.EffectivePan,
                            Sample = new Sample(trackModel.Instrument, (Pitch)Enum.Parse(typeof(Pitch), noteModel.NotePitch, true))
                        }).ToList())
                }).ToList()
            };
        }

        int length = 0;
        try
        {
            length = (int)((Math.Floor(Project.Tracks.SelectMany(track => track.Notes.Keys).Max() / 16.0) + 1) * 16);
        }
        catch
        {
            length = 0;
        }
        Project.Length = length != 0 ? length : 16;

        TrackGrid.Children.Clear();
        TrackGrid.RowDefinitions.Clear();
        TrackLabelsGrid.Children.Clear();
        TrackLabelsGrid.RowDefinitions.Clear();

        foreach (var track in Project.Tracks)
        {
            Random random = new Random();
            var bc = Color.FromRgb(random.Next(130, 170), random.Next(130, 170), random.Next(130, 170));



            var VolStr = track.IsMuted ? "Заглушен" : $"{track.Volume}% громкости";
            var trackLabel = new Label
            {
                Text = $"Имя: {track.Name}\n{VolStr}",
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                HeightRequest = 52,
                TextColor = Colors.Black,
                BackgroundColor = bc,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(5, 0, 0, 0)
            };

            var instrumentImage = new Image
            {
                Source = $"{track.Instrument.ToLower()}.png",
                HeightRequest = 50,
                WidthRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };

            var grid = new Grid
            {
                BackgroundColor = bc,
                Children = { trackLabel, instrumentImage },
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            TrackLabelsGrid.AddRowDefinition(new RowDefinition(52));
            TrackLabelsGrid.Children.Add(grid);
            TrackLabelsGrid.SetRow(grid, Project.Tracks.IndexOf(track));
        }

        ProjNameLabel.Text = $"Project: {Project.Name}";
        ProjBpmLabel.Text = $"Bpm: {Project.Bpm}";

        Project.Playlist = this;

        Project.IsOptimizationEnabled = OptimizeCheckBox.IsChecked;

        PlaylistUpdate();
    }

    public void SettingsClicked(object sender, EventArgs e)
    {
        var popup = new SettingsPopup(Project);
        popup.SettingsApplied += OnSettingsApplied;
        this.ShowPopup(popup);
    }

    private void OnSettingsApplied()
    {
        ProjNameLabel.Text = $"Project: {Project.Name}";
        ProjBpmLabel.Text = $"Bpm: {Project.Bpm}";
    }

    public void PlayClicked(object sender, EventArgs e)
    {
        foreach (var cell in Line.Children.Cast<Frame>())
        {
            cell.BackgroundColor = Colors.White;
        }

        ButtonActivation(false, true);
        Project.PlayFromPosition(0);
    }

    public void StopClicked(object sender, EventArgs e)
    {
        Project.Stop();
        ButtonActivation(true, false);
    }

    public void ButtonActivation(bool play, bool stop)
    {
        Application.Current.Dispatcher.Dispatch(() =>
        {
            PlayButt.IsEnabled = play;
            StopButt.IsEnabled = stop;
        });
    }

    public void Optimize(object sender, CheckedChangedEventArgs e)
    {
        OptimizationEvent?.Invoke(e.Value);
    }

}