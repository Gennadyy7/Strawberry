using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Models
{
    public class TrackModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Instrument { get; set; }
        public int Volume { get; set; }
        public string IsMuted { get; set; }
        public int Pan { get; set; }
        public ICollection<NoteModel> Notes { get; set; }
    }
}
