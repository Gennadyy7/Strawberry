using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Entities
{
    public class Track
    {
        public string Name { get; set; }
        public string Instrument { get; set; }
        public int Volume { get; set; } = 80;
        public bool IsMuted { get; set; } = false;
        public int Pan { get; set; } = 0;
        public Dictionary<int, List<Note>> Notes { get; set; }

        public Track()
        {
            Notes = new Dictionary<int, List<Note>>();
        }
        public Track(string name, string instrument)
        {
            Name = name;
            Instrument = instrument;
            Notes = new Dictionary<int, List<Note>>();
        }
        public void SetName(string name)
        {
            Name = name;
        }
        public void Mute()
        {
            IsMuted = true;
        }
        public void Unmute()
        {
            IsMuted = false;
        }
        public void SetVolume(int volume)
        {
            Volume = volume;
            foreach (var noteList in Notes.Values)
            {
                foreach (var note in noteList)
                {
                    note.EffectiveVolume = (int)((note.Volume * Volume) / 100.0);
                }
            }
        }
        public void SetPan(int pan)
        {
            Pan = pan;
            foreach (var noteList in Notes.Values)
            {
                foreach (var note in noteList)
                {
                    note.EffectivePan = Math.Clamp(note.Pan + Pan, -100, 100);
                }
            }
        }
        public void AddNote(Note note)
        {
            note = new Note(note, Instrument);
            note.EffectiveVolume = (int)((note.Volume * Volume) / 100.0);
            note.EffectivePan = Math.Clamp(note.Pan + Pan, -100, 100);
            if (Notes.TryGetValue(note.Position, out var noteList))
            {
                noteList.Add(note);
            }
            else
            {
                Notes[note.Position] = new List<Note> { note };
            }
        }
        public void AddNote(Pitch notePitch, int duration, int position, int volume, int pan)
        {
            Note note = new Note(notePitch, duration, position, volume, pan, Instrument);
            note.EffectiveVolume = (int)((note.Volume * Volume) / 100.0);
            note.EffectivePan = Math.Clamp(note.Pan + Pan, -100, 100);
            if (Notes.TryGetValue(note.Position, out var noteList))
            {
                noteList.Add(note);
            }
            else
            {
                Notes[note.Position] = new List<Note> { note };
            }
        }
        public void RemoveNote(Note note)
        {
            if (Notes.TryGetValue(note.Position, out var noteList))
            {
                noteList.Remove(note);

                if (noteList.Count == 0)
                {
                    Notes.Remove(note.Position);
                }
            }
        }
        public void RemoveNote(Pitch pitch, int position)
        {
            if (Notes.TryGetValue(position, out var noteList))
            {
                Note noteToRemove = noteList.FirstOrDefault(note => note.NotePitch == pitch);
                if (noteToRemove != null)
                {
                    noteList.Remove(noteToRemove);

                    if (noteList.Count == 0)
                    {
                        Notes.Remove(position);
                    }
                }
            }
        }
        public void PlayNotesAtPosition(int position, int bpm)
        {
            if (Notes.TryGetValue(position, out var noteList))
            {
                foreach (var note in noteList)
                {
                    note.PlayNote(bpm);
                }
            }
        }
    }
}
