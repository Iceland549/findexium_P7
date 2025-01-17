using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P7CreateRestApi.Repositories
{
    public class CurvePointRepository
    {
        private readonly LocalDbContext _context;

        public CurvePointRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllAsync()
        {
            return await _context.CurvePoints.ToListAsync();
        }

        public async Task<CurvePoint?> GetByIdAsync(int id)
        {
            return await _context.CurvePoints.FindAsync(id);
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            await _context.CurvePoints.AddAsync(curvePoint);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CurvePoint curvePoint)
        {
            var existingCurvePoint = await _context.CurvePoints.FindAsync(curvePoint.Id);
            if (existingCurvePoint == null)
            {
                throw new KeyNotFoundException("CurvePoint not found");
            }
            _context.Entry(existingCurvePoint).CurrentValues.SetValues(curvePoint);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var curvePoint = await _context.CurvePoints.FindAsync(id);
            if (curvePoint == null)
            {
                throw new KeyNotFoundException("CurvePoint not found");
            }
            _context.CurvePoints.Remove(curvePoint);
            await _context.SaveChangesAsync();
        }
    }
}
