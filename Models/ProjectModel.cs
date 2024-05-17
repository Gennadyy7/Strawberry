using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TrackModel> Tracks { get; set; }
        public int Bpm { get; set; }
    }
}
