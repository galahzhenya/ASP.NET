using System.ComponentModel.DataAnnotations.Schema;

namespace MyMusic.Models.Data
{
    [Table("tblSongs")]
    public class SongDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Lyrics { get; set; }
        
    }
}