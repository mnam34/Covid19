using Interface;
namespace DataAccess
{
    public class Repository : IRepository
    {
        private DataContext _context;
        public Repository(DataContext context)
        {
            _context = context;
        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_context);
        }
    }
}
