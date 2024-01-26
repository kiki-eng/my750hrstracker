namespace _750HrsTracker.Repositories.Interfaces
{
    public interface IWrapper : IDisposable
    {

        IPropertyRepository PropertyRepository { get; }
        void Save();
        Task<int> SaveChangesAsync();
    }
}
