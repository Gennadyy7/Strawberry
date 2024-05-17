using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Models
{
    public class NoteModel
    {
        public int Id { get; set; }
        public string NotePitch { get; set; }
        public int Duration { get; set; }
        public int Position { get; set; }
        public int Volume { get; set; }
        public int EffectiveVolume { get; set; }
        public int Pan { get; set; }
        public int EffectivePan { get; set; }
    }
}
