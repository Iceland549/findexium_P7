using P7CreateRestApi.Controllers;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P7CreateRestApi.Repositories
{
    public class RuleNameRepository
    {
        private readonly LocalDbContext _context;

        public RuleNameRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RuleName>> GetAllAsync()
        {
            return await _context.RuleNames.ToListAsync();
        }

        public async Task<RuleName> GetByIdAsync(int id)
        {
            return await _context.RuleNames.FindAsync(id);
        }

        public async Task AddAsync(RuleName ruleName)
        {
            await _context.RuleNames.AddAsync(ruleName);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RuleName ruleName)
        {
            var existingRuleName = await _context.RuleNames.FindAsync(ruleName.Id);
            if (existingRuleName == null)
            {
                throw new KeyNotFoundException("RuleName not found");
            }
            _context.Entry(existingRuleName).CurrentValues.SetValues(ruleName);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ruleName = await _context.RuleNames.FindAsync(id);
            if (ruleName == null)
            {
                throw new KeyNotFoundException("RuleName not found");
            }
            _context.RuleNames.Remove(ruleName);
            await _context.SaveChangesAsync();
        }
    }
}
