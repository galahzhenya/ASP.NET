using System.Data.Entity;

namespace GoodMusic.Models.Data
{
    public class GoodMusicDb : DbContext
    {
        public DbSet<SongDTO> Songs { get; set; }

    }
}