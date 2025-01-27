using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using P7CreateRestApi.Dtos;


namespace P7CreateRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly TradeRepository _repository;

        public TradeController(TradeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trade>>> GetAllAsync()
        {
            var trades = await _repository.GetAllAsync();
            return Ok(trades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetByIdAsync(int id)
        {
            var trade = await _repository.GetByIdAsync(id);
            if (trade == null)
            {
                return NotFound();
            }
            return trade;
        }

        [HttpPost]
        public async Task<ActionResult<Trade>> CreateAsync([FromBody] Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(trade);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = trade.TradeId }, trade);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TradeDto tradeDto)
        {
            if (id != tradeDto.TradeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalTrade = await _repository.GetByIdAsync(id);
                if (originalTrade == null)
                {
                    return NotFound();
                }
                originalTrade.Account = tradeDto.Account;
                originalTrade.AccountType = tradeDto.AccountType;
                originalTrade.BuyQuantity = tradeDto.BuyQuantity;
                originalTrade.SellQuantity = tradeDto.SellQuantity;
                originalTrade.BuyPrice = tradeDto.BuyPrice;
                originalTrade.SellPrice = tradeDto.SellPrice;
                originalTrade.TradeSecurity = tradeDto.TradeSecurity;
                originalTrade.TradeStatus = tradeDto.TradeStatus;
                originalTrade.Trader = tradeDto.Trader;
                originalTrade.Benchmark = tradeDto.Benchmark;
                originalTrade.Book  = tradeDto.Book;
                originalTrade.DealName = tradeDto.DealName;
                originalTrade.DealType = tradeDto.DealType;
                originalTrade.SourceListId = tradeDto.SourceListId;
                originalTrade.Side = tradeDto.Side;

                await _repository.UpdateAsync(originalTrade);
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
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
