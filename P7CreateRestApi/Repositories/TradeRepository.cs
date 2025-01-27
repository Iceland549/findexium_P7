using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P7CreateRestApi.Repositories
{
    public class TradeRepository
    {
        private readonly LocalDbContext _context;

        public TradeRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trade>> GetAllAsync()
        {
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade?> GetByIdAsync(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task AddAsync(Trade trade)
        {
            await _context.Trades.AddAsync(trade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trade trade)
        {
            var existingTrade = await _context.Trades.FindAsync(trade.TradeId);
            if (existingTrade == null)
            {
                throw new KeyNotFoundException("Trade not found");
            }
            _context.Entry(existingTrade).CurrentValues.SetValues(trade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
            {
                throw new KeyNotFoundException("Trade not found");
            }
            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
        }
    }
}
