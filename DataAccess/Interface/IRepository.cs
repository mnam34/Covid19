namespace Interface
{
    public interface IRepository
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
    }
}
