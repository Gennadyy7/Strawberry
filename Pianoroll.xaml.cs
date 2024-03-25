using CommunityToolkit.Maui.Views;
using Strawberry.Entities;

namespace Strawberry;

public partial class Pianoroll : ContentPage
{
    public Dictionary<int, List<Button>> ButtonNotes { get; set; }
    public int Length { get; set; }

    public bool CanPaint { get; set; } = false;
    public int Volume { get; set; } = 80;
    public Track Track { get; set; }
    public Project Project { get; set; }
    public Pianoroll()
    {
        InitializeComponent();

        KeysScrollView.Scrolled += (s, e) =>
        {
            NoteScrollView.ScrollToAsync(0, KeysScrollView.ScrollY, true);
        };

        ButtonNotes = new Dictionary<int, List<Button>>();
        Project = new Project("Test", 60);
        Track = new Track("Test track", "Piano");
        Project.AddTrack(Track);
        Length = 2;

        for (int row = 0; row < 60; row++)
        {
            for (int col = 0; col < 16; col++)
            {
                Button button = new Button
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    Text = string.Empty,
                    CornerRadius = 1
                };

                button.BackgroundColor = Colors.White;

                if (col % 4 == 0)
                {
                    button.BackgroundColor = Colors.LightSalmon;
                }

                if (row % 12 == 1 || row % 12 == 3 || row % 12 == 5 || row % 12 == 8 || row % 12 == 10)
                {
                    button.BackgroundColor = Colors.LightGray;
                }

                button.AutomationId = $"cell_{row}_{col}";
                button.Clicked += CellPressed;

                NoteGrid.Add(button, col, row);

                if (!ButtonNotes.ContainsKey(col))
                {
                    ButtonNotes[col] = new List<Button>();
                }

                ButtonNotes[col].Add(button);
            }
        }
    }

    public async void ChangeVolume(object sender, EventArgs e)
    {
        //string vol = await DisplayPromptAsync("Громкость дорожки", "Введите целое значение громкости (от 0% до 100%)", initialValue: Volume.ToString(), maxLength: 3, keyboard: Keyboard.Numeric, cancel: null);
        //Volume = Math.Abs((int)double.Parse((vol == null)? Volume.ToString() : vol));
        //var popup = new VolumeSlider(this);
        //this.ShowPopup(popup);
        await Project.PlayFromPosition(0);
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
        string l = await DisplayActionSheet("Выберите длину ноты:", null, null, "1", "2", "4", "8", "16");
        Length = Int32.Parse((l == null) ? Length.ToString() : l);
    }
    public void CellPressed(object sender, EventArgs e)
    {
        if (CanPaint)
        {
            Button s = (Button)sender;
            int column = Grid.GetColumn(s);
            int row = Grid.GetRow(s);

            if (s.BackgroundColor == Colors.DarkOrange)
            {
                s.BackgroundColor = Colors.White;

                if (column % 4 == 0)
                {
                    s.BackgroundColor = Colors.LightSalmon;
                }

                if (row % 12 == 1 || row % 12 == 3 || row % 12 == 5 || row % 12 == 8 || row % 12 == 10)
                {
                    s.BackgroundColor = Colors.LightGray;
                }

                s.WidthRequest = 20;
                int tempLenght = NoteGrid.GetColumnSpan(s);
                NoteGrid.SetColumnSpan(s, 1);

                for (int i = 1; i < Math.Min(tempLenght, 16 - column); i++)
                {
                    Button removedButton = ButtonNotes[column + i][row];
                    NoteGrid.Children.Add(removedButton);
                }

                Track.RemoveNote((Pitch)row, column);
            }
            else
            {
                int maxSpan = 1;
                for (int i = 1; i < Length; i++)
                {
                    if (column + i < 16 && ButtonNotes[column + i][row].BackgroundColor != Colors.DarkOrange)
                    {
                        maxSpan = 1 + i;
                    }
                    else
                    {
                        break;
                    }
                }

                s.BackgroundColor = Colors.DarkOrange;
                s.WidthRequest = s.Width * maxSpan;
                NoteGrid.SetColumnSpan(s, maxSpan);

                for (int i = 1; i < maxSpan; i++)
                {
                    Button overlappedButton = ButtonNotes[column + i][row];
                    NoteGrid.Children.Remove(overlappedButton);
                }

                Track.AddNote((Pitch)row, maxSpan, column, 100, 0);
            }
        }
    }
}