using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace P7CreateRestApi.Repositories
{
    public class BidListRepository : IBidListRepository
    {
        private readonly LocalDbContext _context;

        public BidListRepository(LocalDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BidList>> GetAllAsync()
        {
            return await _context.BidLists.ToListAsync();
        }
        public async Task<BidList?> GetByIdAsync(int id)
        {
            return await _context.BidLists.FindAsync(id);
        }
        public async Task AddAsync(BidList bidList)
        {
            await _context.BidLists.AddAsync(bidList);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(BidList bidList)
        {
            var existingBidList = await _context.BidLists.FindAsync(bidList.BidListId) ?? throw new KeyNotFoundException("Bidlist not found");
            _context.Entry(existingBidList).CurrentValues.SetValues(bidList);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var bidList = await _context.BidLists.FindAsync(id) ?? throw new KeyNotFoundException("BidList not found");
            _context.BidLists.Remove(bidList);
            await _context.SaveChangesAsync();
        }
    }
}
