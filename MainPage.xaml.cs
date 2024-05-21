using Strawberry.Entities;

namespace Strawberry
{
    public partial class MainPage : ContentPage
    {
        public Project Project { get; set; }
        public MainPage()
        {
            InitializeComponent();
            Project = new Project("Test", 100);
            Track track = new Track("Simple Piano", "Piano");
            var chords = Harmonizer.GenerateDiatonicTriads(Pitch.C3, HarmonyMode.Major, 4, track.Instrument);
            int i = 0;
            foreach (var ch in chords)
            {
                Harmonizer.SetTriadPosition(ch.Value, i);
                foreach (var n in ch.Value)
                {
                    track.AddNote(n);
                }
                i += 4;
            }
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            Project.PlayFromPosition(0);
        }
    }

}
