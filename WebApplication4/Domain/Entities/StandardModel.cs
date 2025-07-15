using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApp.Domain.Models
{
    public abstract class StandardModel
    {
        [Key]
        public long Id { get; private set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; private set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; private set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public void SetIsDeleted()
        {
            IsDeleted = true;
        }

        public void SetCreatedAt()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUpdatedAt()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
