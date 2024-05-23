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
            Project.AddTrack(track);
            /*track.AddNote(Pitch.C4, 2, 0, 80, 0);
            track.AddNote(Pitch.D4, 2, 2, 80, 0);
            track.AddNote(Pitch.E4, 4, 4, 80, 0);

            track.AddNote(Pitch.E4, 2, 8, 80, 0);
            track.AddNote(Pitch.D4, 2, 10, 80, 0);
            track.AddNote(Pitch.C4, 4, 12, 80, 0);

            track.AddNote(Pitch.C4, 2, 16, 80, 0);
            track.AddNote(Pitch.D4, 2, 18, 80, 0);
            track.AddNote(Pitch.E4, 2, 20, 80, 0);
            track.AddNote(Pitch.E4, 2, 22, 80, 0);
            track.AddNote(Pitch.E4, 2, 24, 80, 0);
            track.AddNote(Pitch.D4, 2, 26, 80, 0);
            track.AddNote(Pitch.C4, 4, 28, 80, 0);

            track.AddNote(Pitch.C4, 2, 32, 80, 0);*/

            track.AddNote(Pitch.C5, 2, 0, 80, 0);
            track.AddNote(Pitch.C5, 2, 2, 80, 0);
            track.AddNote(Pitch.C5, 2, 4, 80, 0);
            track.AddNote(Pitch.C5, 2, 6, 80, 0);
            track.AddNote(Pitch.C5, 2, 8, 80, 0);
            track.AddNote(Pitch.C5, 2, 10, 80, 0);
            track.AddNote(Pitch.C5, 2, 12, 80, 0);
            track.AddNote(Pitch.C5, 2, 14, 80, 0);
            track.AddNote(Pitch.Eb5, 2, 16, 80, 0);
            track.AddNote(Pitch.C5, 2, 18, 80, 0);
            track.AddNote(Pitch.C5, 2, 20, 80, 0);
            track.AddNote(Pitch.Eb5, 2, 22, 80, 0);
            track.AddNote(Pitch.Db5, 2, 24, 80, 0);
            track.AddNote(Pitch.Eb5, 2, 26, 80, 0);
            track.AddNote(Pitch.Db5, 2, 28, 80, 0);
            track.AddNote(Pitch.Eb5, 2, 30, 80, 0);
            track.AddNote(Pitch.Bb5, 4, 32, 80, 0);

            var harmonyChords = Harmonizer.Harmonize(track, Pitch.Bb3, HarmonyMode.Minor, 2);
            Track harmonyTrack = new Track("Harmony", "Piano");
            harmonyTrack.SetNotes(harmonyChords);
            Project.AddTrack(harmonyTrack);

            Project.Length = 48;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            Project.PlayFromPosition(0);
        }
    }

}
