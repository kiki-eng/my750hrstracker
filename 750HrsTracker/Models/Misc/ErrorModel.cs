using _750HrsTracker.Enums;

namespace _750HrsTracker.Models.Misc
{
    public class ErrorModel
    {
        public bool success { get; set; } = false;
        public ErrorSeverity severity { get; set; } = ErrorSeverity.HIGH;
        public string? message { get; set; }
        public string? data { get; set; }
    }

}
