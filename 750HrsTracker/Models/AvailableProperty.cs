using System.ComponentModel.DataAnnotations;

namespace _750HrsTracker.Models
{
    public class AvailableProperty : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
    }
}
