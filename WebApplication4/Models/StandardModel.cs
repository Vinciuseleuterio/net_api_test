using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Models
{
    public class StandardModel
    {
        [Key]
        public long Id { get; private set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; private set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; private set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; private set; }

        public void Delete()
        {
            IsDeleted = true;
        }

        public void Created()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void Updated()
        {
            UpdatedAt = DateTime.UtcNow;

        }
    }
}
