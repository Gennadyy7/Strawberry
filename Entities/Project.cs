using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Entities
{
    public class Project
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
        public int SliderPos { get; set; } = 0;
        public int Bpm { get; set; } = 120;
        public CancellationTokenSource cts { get; set; }
        public Project()
        {
            Tracks = new List<Track>();
        }
        public Project(string name, int bpm)
        {
            Name = name;
            Bpm = bpm;
            Tracks = new List<Track>();
        }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }
        public void RemoveTrack(Track track)
        {
            Tracks.Remove(track);
        }
        public void SetBpm(int bpm)
        {
            Bpm = bpm;
        }
        public async Task PlayFromPosition(int position)
        {
            cts = new CancellationTokenSource();
            SliderPos = position;
            while (SliderPos < 16)
            {
                foreach (var track in Tracks)
                {
                    track.PlayNotesAtPosition(SliderPos, Bpm);
                }
                await Task.Delay((int)((15.0 / Bpm) * 1000));
                SliderPos++;
                if (cts.Token.IsCancellationRequested)
                {
                    break;
                }
            }
        }
        public void Stop()
        {
            cts?.Cancel();
            foreach (var track in Tracks)
            {
                foreach (var position in track.Notes.Keys)
                {
                    if (position <= SliderPos)
                    {
                        foreach (var note in track.Notes[position])
                        {
                            note.Sample.Sound.Stop();
                        }
                    }
                }
            }
        }
    }
}
