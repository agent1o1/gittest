using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Index(nameof(ImdbID), Name = "IDX_ImdbID", IsUnique = true)]
    public class Favorites : BaseEntity
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string ImdbID { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
    }
}
