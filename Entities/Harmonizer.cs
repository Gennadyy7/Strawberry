using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Entities
{
    public class Harmonizer
    {
        public static Dictionary<Pitch, List<Note>> GenerateDiatonicTriads(Pitch tonic, HarmonyMode mode, int chordsPerBar, string instrument)
        {
            var diatonicTriads = new Dictionary<Pitch, List<Note>>();

            int[] intervals;
            switch (mode)
            {
                case HarmonyMode.Major:
                    intervals = [0, 2, 4, 5, 7, 9, 11];
                    break;
                case HarmonyMode.Minor:
                    intervals = [0, 2, 3, 5, 7, 8, 10];
                    break;
                case HarmonyMode.HarmonicMinor:
                    intervals = [0, 2, 3, 5, 7, 8, 11];
                    break;
                default:
                    throw new ArgumentException("Invalid harmony mode.");
            }

            for (int i = 0; i < 6; i++)
            {
                var thirdPitch = tonic - intervals[i] - intervals[i + 2];
                var fifthPitch = tonic - intervals[i] - intervals[i + 4];

                if (i > 2)
                {
                    fifthPitch += 12;
                    
                    if (i > 4)
                    {
                        thirdPitch += 12;
                    }
                }

                var triad = new List<Note>
                {
                    new Note(tonic - intervals[i] + 12, 16 / chordsPerBar, 0, 80, 0, instrument),
                    new Note(tonic - intervals[i], 16 / chordsPerBar, 0, 80, 0, instrument),
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
    }
}
