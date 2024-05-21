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
    }
}
