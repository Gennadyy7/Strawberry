using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Strawberry.Entities;

namespace Strawberry;

public partial class Pianoroll : ContentPage
{
    public int Length { get; set; }

    public bool CanPaint { get; set; } = false;
    public int Volume { get; set; } = 80;
    public Track Track { get; set; } = null;
    public Project Project { get; set; } = null;

    public static event Action BackEvent;
    protected Pianoroll()
    {
        InitializeComponent();

        KeysScrollView.Scrolled += (s, e) =>
        {
            NoteScrollView.ScrollToAsync(0, KeysScrollView.ScrollY, true);
        };

        Length = 2;
    }

    public Pianoroll(Project project, int trackIndex):this()
    {
        Project = project;
        Track = project.Tracks[trackIndex];
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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
        await InitializeAsync();
        await FillTheNotes();
        Content = page;
    }

    public async Task InitializeAsync()
    {
        for (int row = 0; row < 60; row++)
        {
            NoteGrid.RowDefinitions.Add(new RowDefinition(25));
            for (int col = 0; col < 16; col++)
            {
                Frame button = new Frame
                {
                    MinimumWidthRequest = 25,
                    WidthRequest = 25,
                    HeightRequest = 25,
                    CornerRadius = 1,
                    BorderColor = Colors.Black
                };

                button.BackgroundColor = Colors.White;

                if (col % 4 == 0)
                {
                    button.BackgroundColor = Color.FromRgb(255, 220, 182);
                }

                if (row % 12 == 1 || row % 12 == 3 || row % 12 == 5 || row % 12 == 8 || row % 12 == 10)
                {
                    button.BackgroundColor = Colors.LightGray;
                }

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) => {
                    CellPressed(s, e);
                };
                button.GestureRecognizers.Add(tapGestureRecognizer);

                NoteGrid.Add(button, column: col);
                NoteGrid.SetRow(button, row);

                await Task.Delay(0);
            }
        }
    }

    public async Task FillTheNotes()
    {
        foreach (var noteGroup in Track.Notes)
        {
            foreach (var note in noteGroup.Value)
            {
                var button = NoteGrid.Children
                    .OfType<Frame>()
                    .FirstOrDefault(b => Grid.GetRow(b) == (int)note.NotePitch && Grid.GetColumn(b) == noteGroup.Key);
                button.BackgroundColor = Colors.Black;
                button.WidthRequest = button.WidthRequest * note.Duration;
                NoteGrid.SetColumnSpan(button, note.Duration);

                for (int i = 1; i < note.Duration; i++)
                {
                    Frame overlappedButton = NoteGrid.Children
                    .OfType<Frame>()
                    .FirstOrDefault(b => Grid.GetRow(b) == (int)note.NotePitch && Grid.GetColumn(b) == noteGroup.Key + i);
                    NoteGrid.Children.Remove(overlappedButton);
                }

                await Task.Delay(0);
            }
        }
    }
    public void ChangeVolume(object sender, EventArgs e)
    {
        //string vol = await DisplayPromptAsync("Громкость дорожки", "Введите целое значение громкости (от 0% до 100%)", initialValue: Volume.ToString(), maxLength: 3, keyboard: Keyboard.Numeric, cancel: null);
        //Volume = Math.Abs((int)double.Parse((vol == null)? Volume.ToString() : vol));
        //var popup = new VolumeSlider(this);
        //this.ShowPopup(popup);
        Project.PlayFromPosition(0);
    }

    public void GoBack(object sender, EventArgs e)
    {
        BackEvent?.Invoke();
        SwitchToPlaylist();
    }

    protected override bool OnBackButtonPressed()
    {
        BackEvent?.Invoke();
        SwitchToPlaylist();
        return true;
    }

    public async void SwitchToPlaylist()
    {
        await Navigation.PopAsync(false);
    }

    public void SettingsClicked(object sender, EventArgs e)
    {
        Project.Stop();
    }
    public void SelectPainting(object sender, EventArgs e)
    {
        if (!CanPaint)
        {
            CanPaint = true;
            PaintButt.Background = Colors.DarkOrange;
        }
        else
        {
            CanPaint = false;
            PaintButt.Background = Colors.DarkRed;
        }
    }
    public async void ResizeNote(object sender, EventArgs e)
    {
        string l = await Application.Current.MainPage.DisplayActionSheet("Выберите длину ноты:", null, null, "1", "2", "4", "8", "16");
        Length = Int32.Parse((l == null) ? Length.ToString() : l);
    }
    public void CellPressed(object sender, EventArgs e)
    {
        if (CanPaint)
        {
            Frame s = (Frame)sender;
            int column = Grid.GetColumn(s);
            int row = Grid.GetRow(s);

            if (s.BackgroundColor == Colors.Black)
            {
                s.BackgroundColor = Colors.White;

                if (column % 4 == 0)
                {
                    s.BackgroundColor = Color.FromRgb(255, 220, 182);
                }
                
                if (row % 12 == 1 || row % 12 == 3 || row % 12 == 5 || row % 12 == 8 || row % 12 == 10)
                {
                    s.BackgroundColor = Colors.LightGray;
                }

                s.WidthRequest = 25;
                int tempLenght = NoteGrid.GetColumnSpan(s);
                NoteGrid.SetColumnSpan(s, 1);

                for (int i = 1; i < Math.Min(tempLenght, 16 - column); i++)
                {
                    Frame removedButton = new Frame
                    {
                        MinimumWidthRequest = 25,
                        WidthRequest = 25,
                        HeightRequest = 25,
                        CornerRadius = 1,
                        BorderColor = Colors.Black
                    };

                    removedButton.BackgroundColor = Colors.White;

                    if ((column + i) % 4 == 0)
                    {
                        removedButton.BackgroundColor = Color.FromRgb(255, 220, 182);
                    }
                    
                    if (row % 12 == 1 || row % 12 == 3 || row % 12 == 5 || row % 12 == 8 || row % 12 == 10)
                    {
                        removedButton.BackgroundColor = Colors.LightGray;
                    }

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        CellPressed(s, e);
                    };
                    removedButton.GestureRecognizers.Add(tapGestureRecognizer);

                    NoteGrid.Add(removedButton, column: column + i);
                    NoteGrid.SetRow(removedButton, row);
                }

                Track.RemoveNote((Pitch)row, column);
            }
            else
            {
                int maxSpan = 1;
                for (int i = 1; i < Length; i++)
                {
                    Frame overlappedButton = NoteGrid.Children
                    .OfType<Frame>()
                    .FirstOrDefault(b => Grid.GetRow(b) == row && Grid.GetColumn(b) == column + i);
                    if (column + i < 16 && overlappedButton != null && overlappedButton.BackgroundColor != Colors.Black)
                    {
                        maxSpan = 1 + i;
                    }
                    else
                    {
                        break;
                    }
                }

                s.BackgroundColor = Colors.Black;
                s.WidthRequest = s.Width * maxSpan;
                NoteGrid.SetColumnSpan(s, maxSpan);

                for (int i = 1; i < maxSpan; i++)
                {
                    Frame overlappedButton = NoteGrid.Children
                    .OfType<Frame>()
                    .FirstOrDefault(b => Grid.GetRow(b) == row && Grid.GetColumn(b) == column + i);
                    NoteGrid.Children.Remove(overlappedButton);
                }

                Track.AddNote((Pitch)row, maxSpan, column, 100, 0);
            }
        }
    }

    public void NextClicked(object sender, EventArgs e)
    {
    }

    public void PrevClicked(object sender, EventArgs e)
    {
    }
}