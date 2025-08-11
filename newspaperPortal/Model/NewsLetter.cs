using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace newspaperPortal.Model
{
    [Table("Newsletters")]
    public class NewsLetter : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; } 

        [Column("real_time")]
        public int ReadTime { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
