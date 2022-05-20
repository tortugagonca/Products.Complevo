using System.Threading.Tasks;

namespace Products.Complevo.Infrastructure.Data.Repository
{
    public abstract class BaseRepository 
    {
        protected readonly ProductsComplevoContext _context;

        public BaseRepository(ProductsComplevoContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync() 
            => await _context.SaveChangesAsync();
    }
}