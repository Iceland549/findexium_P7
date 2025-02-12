using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface IBidListRepository
    {
        Task<IEnumerable<BidList>> GetAllAsync();
        Task<BidList?> GetByIdAsync(int id);
        Task AddAsync(BidList bidList);
        Task UpdateAsync(BidList bidList);
        Task DeleteAsync(int id);
    }

}
