using System.Data.Entity;

namespace MyMusic.Models.Data
{
    public class MyMusicDb : DbContext
    {
        public DbSet<SongDTO> Songs { get; set; }
    }
}