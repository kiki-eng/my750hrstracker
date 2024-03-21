using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
      
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            ModifiedAt = DateTime.Now;
        }
    }
}
