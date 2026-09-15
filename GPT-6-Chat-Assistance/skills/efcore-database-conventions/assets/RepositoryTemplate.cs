using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace <ProjectName>Core.Repository
{
    public class ExampleRepository : IExampleRepository
    {
        private readonly ApplicationDbContext _context;

        public ExampleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExampleEntity> GetByIdAsync(int id)
        {
            return await _context.Examples.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<ExampleEntity>> GetAllActiveAsync()
        {
            // Always use AsNoTracking for read-only lists
            return await _context.Examples
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }
    }
}
