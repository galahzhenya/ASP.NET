using System.ComponentModel.DataAnnotations.Schema;


namespace GoodMusic.Models.Data
{
    [Table("tblSongs")]
    public class SongDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}