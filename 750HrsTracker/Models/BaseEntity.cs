using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public BaseEntity()
        {
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}
