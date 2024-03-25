using Strawberry.Entities;

namespace Strawberry
{
    public partial class MainPage : ContentPage
    {
        public Project Project { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Project = new Project("Test", 60);
            Track track = new Track("Simple Piano", "Piano");
            track.AddNote(Pitch.E3, 1, 0, 100, 0);
            track.AddNote(Pitch.E3, 1, 1, 100, 0);
            track.AddNote(Pitch.E3, 1, 2, 100, 0);
            track.AddNote(Pitch.E3, 1, 3, 100, 0);
            track.AddNote(Pitch.E3, 1, 4, 100, 0);
            track.AddNote(Pitch.E3, 1, 5, 100, 0);
            track.AddNote(Pitch.E3, 1, 6, 100, 0);
            track.AddNote(Pitch.E3, 1, 7, 100, 0);
            track.AddNote(Pitch.E3, 1, 8, 100, 0);
            track.AddNote(Pitch.E3, 1, 9, 100, 0);
            track.AddNote(Pitch.E3, 1, 10, 100, 0);
            track.AddNote(Pitch.E3, 1, 11, 100, 0);
            track.AddNote(Pitch.E3, 1, 12, 100, 0);
            track.AddNote(Pitch.E3, 1, 13, 100, 0);
            track.AddNote(Pitch.E3, 1, 14, 100, 0);
            track.AddNote(Pitch.E3, 1, 15, 100, 0);
            Project.AddTrack(track);
            Track track2 = new Track("Another Piano", "Piano");
            track2.AddNote(Pitch.D3, 4, 0, 100, 0);
            track2.AddNote(Pitch.D3, 4, 4, 100, 0);
            track2.AddNote(Pitch.D3, 4, 8, 100, 0);
            track2.AddNote(Pitch.D3, 4, 12, 100, 0);
            Project.AddTrack(track2);
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var t = Project.PlayFromPosition(0);
            //await Task.Delay(1300);
            //Project.Stop();
            await Task.WhenAll(t);
        }
    }

}
