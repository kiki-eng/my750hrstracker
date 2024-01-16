namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IWrapper : IDisposable
    {

        void Save();
        Task<int> SaveChangesAsync();
    }
}
