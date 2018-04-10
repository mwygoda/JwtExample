namespace JWTExample.DataAccess.Implementation
{
    public class BaseRepository
    {
        protected readonly ExampleContext _context;

        protected BaseRepository(ExampleContext context)
        {
            _context = context;
        }
    }
}
