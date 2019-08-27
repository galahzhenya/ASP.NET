using System.ComponentModel;

namespace MyMusic.Models.Data.ViewModels
{
    public class SongVM
    {
        public SongVM()
        {

        }

        public SongVM(SongDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            Lyrics = row.Lyrics;
            GroupName = row.GroupName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Lyrics { get; set; }
        [DisplayName("Group")]
        public string GroupName { get; set; }
    }
}