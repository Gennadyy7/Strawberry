using Microsoft.EntityFrameworkCore;
using Strawberry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strawberry
{
    public class ApplicationContext : DbContext
    {
        public DbSet<NoteModel> Notes { get; set; }
        public DbSet<TrackModel> Tracks { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "strawberry.db");
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }
}
