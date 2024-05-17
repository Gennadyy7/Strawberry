using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Entities
{
    public class Note
    {
        public Pitch NotePitch { get; set; }
        public int Duration { get; set; }
        public int Position { get; set; }
        public int Volume { get; set; }
        public int EffectiveVolume { get; set; }
        public int Pan { get; set; }
        public int EffectivePan { get; set; }
        public Sample Sample { get; set; }

        public void PlayNote(int bpm)
        {
            Sample.PlaySound(EffectiveVolume, Duration, EffectivePan, bpm);
        }
        public Note(Pitch notePitch, int duration, int position, int volume, int pan, string sampleName)
        {
            NotePitch = notePitch;
            Duration = duration;
            Position = position;
            Volume = volume;
            Pan = pan;
            Sample = new Sample(sampleName, NotePitch);
        }
        public Note(Note note, string sampleName)
        {
            NotePitch = note.NotePitch;
            Duration = note.Duration;
            Position = note.Position;
            Volume = note.Volume;
            Pan = note.Pan;
            Sample = new Sample(sampleName, NotePitch);
        }

        public Note()
        {
            NotePitch = Pitch.C3;
            Duration = 0;
            Position = 0;
            Volume = 0;
            Pan = 0;
            Sample = new Sample("Piano", NotePitch);
        }
    }
}
