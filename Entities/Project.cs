using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Strawberry.Entities
{
    public class Project
    {
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
        public int SliderPos { get; set; }
        public int Bpm { get; set; }
        public CancellationTokenSource cts { get; set; }

        private System.Timers.Timer _timer;
        public int Length { get; set; } = 16;

        public Playlist Playlist { get; set; }

        public Project(string name, int bpm)
        {
            Name = name;
            Bpm = bpm;
            SliderPos = 0;
            Tracks = new List<Track>();
        }

        public Project()
        {
            Name = string.Empty;
            Bpm = 0;
            SliderPos = 0;
            Tracks = new List<Track>();
        }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }
        public void AddTrack(string trackName, string instrument)
        {
            Tracks.Add(new Track(trackName, instrument));
        }
        public void RemoveTrack(Track track)
        {
            Tracks.Remove(track);
        }
        public void SetBpm(int bpm)
        {
            Bpm = bpm;
        }
        public void PlayFromPosition(int position)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            Stop();
            cts = new CancellationTokenSource();
            SliderPos = position;
            _timer = new System.Timers.Timer((15.0 / Bpm) * 1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (cts.Token.IsCancellationRequested)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            else
            {
                Playlist.ToPosition(SliderPos);
                foreach (var track in Tracks)
                {
                    track.PlayNotesAtPosition(SliderPos, Bpm);
                }
                SliderPos++;
                if (SliderPos >= Length)
                {
                    _timer.Stop();
                    _timer.Dispose();
                    Playlist.ButtonActivation(true, false);
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
