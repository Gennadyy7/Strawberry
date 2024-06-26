﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Entities
{
    public class Harmonizer
    {
        private static readonly Dictionary<string, Pitch> TonicaToPitchMap = new Dictionary<string, Pitch>
        {
            { "C", Pitch.C3 },
            { "Db", Pitch.Db3 },
            { "D", Pitch.D3 },
            { "Eb", Pitch.Eb3 },
            { "E", Pitch.E3 },
            { "F", Pitch.F3 },
            { "Gb", Pitch.Gb3 },
            { "G", Pitch.G3 },
            { "Ab", Pitch.Ab3 },
            { "A", Pitch.A3 },
            { "Bb", Pitch.Bb3 },
            { "B", Pitch.B3 }
        };

        public static Pitch? GetPitchFromTonica(string tonica)
        {
            if (TonicaToPitchMap.TryGetValue(tonica, out var pitch))
            {
                return pitch;
            }

            return null;
        }

        public static Dictionary<Pitch, List<Note>> GenerateDiatonicTriads(Pitch tonic, HarmonyMode mode, int chordsPerBar, string instrument)
        {
            var diatonicTriads = new Dictionary<Pitch, List<Note>>();

            int[] intervals;
            int skipIndex;
            switch (mode)
            {
                case HarmonyMode.Major:
                    intervals = [0, 2, 4, 5, 7, 9, 11];
                    skipIndex = 6;
                    break;
                case HarmonyMode.Minor:
                    intervals = [0, 2, 3, 5, 7, 8, 10];
                    skipIndex = 1;
                    break;
                /*case HarmonyMode.HarmonicMinor:
                    intervals = [0, 2, 3, 5, 7, 8, 11];
                    break;*/
                default:
                    throw new ArgumentException("Invalid harmony mode.");
            }

            for (int i = 0; i < 7; i++)
            {
                if (i == skipIndex)
                {
                    continue;
                }

                var firstPitch = tonic - intervals[i];
                var thirdPitch = tonic - intervals[(i + 2) % 7];
                var fifthPitch = tonic - intervals[(i + 4) % 7];

                if (firstPitch <= Pitch.C4)
                {
                    firstPitch += 12;
                }

                if (thirdPitch <= Pitch.C4)
                {
                    thirdPitch += 12;
                }

                if (fifthPitch <= Pitch.C4)
                {
                    fifthPitch += 12;
                }

                var triad = new List<Note>
                {
                    new Note(firstPitch + 12, 16 / chordsPerBar, 0, 80, 0, instrument),
                    new Note(firstPitch, 16 / chordsPerBar, 0, 80, 0, instrument),
                    new Note(thirdPitch, 16 / chordsPerBar, 0, 80, 0, instrument),
                    new Note(fifthPitch, 16 / chordsPerBar, 0, 80, 0, instrument)
                };

                diatonicTriads[tonic - intervals[i]] = triad;
            }

            return diatonicTriads;
        }

        public static void SetTriadPosition(List<Note> triad, int position)
        {
            foreach (var note in triad)
            {
                note.Position = position;
            }
        }

        private static List<Note> CloneChord(List<Note> originalChord)
        {
            List<Note> clonedChord = new List<Note>();
            foreach (Note originalNote in originalChord)
            {
                Note clonedNote = new Note(originalNote.NotePitch, originalNote.Duration, originalNote.Position, originalNote.Volume, originalNote.Pan, originalNote.Sample.Name);
                clonedChord.Add(clonedNote);
            }
            return clonedChord;
        }


        public static Dictionary<int, List<Note>> Harmonize(Track melodyTrack, Pitch tonic, HarmonyMode mode, int chordsPerBar)
        {
            if (melodyTrack.Notes.Count == 0)
            {
                return null;
            }

            int resolutionPosition = melodyTrack.Notes.Keys.Max();

            if (resolutionPosition % 16 != 0)
            {
                return null;
            }

            var diatonicChords = Harmonizer.GenerateDiatonicTriads(tonic, mode, chordsPerBar, melodyTrack.Instrument);

            var harmonizedChords = new Dictionary<int, List<Note>>();

            int beatsPerChord = 16 / chordsPerBar;

            for (int position = 0; position <= resolutionPosition; position += beatsPerChord)
            {
                if (melodyTrack.Notes.TryGetValue(position, out var notesAtPosition))
                {
                    var matchingChords = diatonicChords.Where(kvp =>
                        notesAtPosition.Count == 1
                            ? kvp.Value.Any(n => ((int)n.NotePitch % 12) == ((int)notesAtPosition.First().NotePitch % 12))
                            : notesAtPosition.Count(note => kvp.Value.Any(n => ((int)n.NotePitch % 12) == ((int)note.NotePitch % 12))) >= notesAtPosition.Count - 1
                    ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);


                    if (matchingChords.Count > 0)
                    {
                        Pitch selectedChordRoot;
                        List<Note> selectedChord;

                        if (position == resolutionPosition)
                        {
                            if (matchingChords.ContainsKey(tonic))
                            {
                                selectedChord = diatonicChords[tonic];
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            Random rnd = new Random();
                            selectedChordRoot = matchingChords.ElementAt(rnd.Next(matchingChords.Count)).Key;
                            selectedChord = matchingChords[selectedChordRoot];
                        }

                        List<Note> clonedChord = CloneChord(selectedChord);
                        Harmonizer.SetTriadPosition(clonedChord, position);


                        harmonizedChords[position] = clonedChord;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            return harmonizedChords;
        }
    }
}
