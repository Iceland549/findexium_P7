using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Dtos;


namespace P7CreateRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidListController : ControllerBase
    {
        private readonly BidListRepository _repository;
        public BidListController(BidListRepository repository)
        {
            _repository = repository; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidList>>> GetAllAsync()
        {
            // TODO: check data valid and save to db, after saving return bid list
            var bidList = await _repository.GetAllAsync();
            return Ok(bidList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidList>> GetByIdAsync(int id)
        {
            var bidList = await _repository.GetByIdAsync(id);
            if (bidList == null)
            {
                return NotFound();
            }
            return bidList;
        }

        [HttpPost]
        public async Task<ActionResult<BidList>> CreateAsync([FromBody]BidList bidList) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(bidList);
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            return CreatedAtAction(nameof(GetByIdAsync), new { id = bidList.BidListId }, bidList);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, BidListDto bidListDto)
        {
            if (id != bidListDto.BidListId)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalBidList = await _repository.GetByIdAsync(id);
                if (originalBidList == null)
                {
                    return NotFound();
                }
                originalBidList.Account = bidListDto.Account;
                originalBidList.BidType = bidListDto.BidType;
                originalBidList.BidQuantity = bidListDto.BidQuantity;
                originalBidList.AskQuantity = bidListDto.AskQuantity;
                originalBidList.Bid = bidListDto.Bid;
                originalBidList.Ask = bidListDto.Ask;
                originalBidList.Benchmark = bidListDto.Benchmark;
                originalBidList.Commentary = bidListDto.Commentary;
                originalBidList.BidSecurity = bidListDto.BidSecurity;
                originalBidList.BidStatus = bidListDto.BidStatus;
                originalBidList.Trader = bidListDto.Trader;
                originalBidList.Book = bidListDto.Book;
                originalBidList.DealName = bidListDto.DealName;
                originalBidList.DealType = bidListDto.DealType;
                originalBidList.SourceListId = bidListDto.SourceListId;
                originalBidList.Side = bidListDto.Side;

                await _repository.UpdateAsync(originalBidList);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}