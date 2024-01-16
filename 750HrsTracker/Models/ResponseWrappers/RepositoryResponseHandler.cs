namespace _750HrsTracker.Models.ResponseWrappers
{
    public class RepositoryResponseHandler<T>
    {
        public List<T>? Records { get; set; }
        public int TotalCount { get; set; }
    }
}
