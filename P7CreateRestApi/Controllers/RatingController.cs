using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Dtos;


namespace P7CreateRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly RatingRepository _repository;

        public RatingController(RatingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetAllAsync()
        {
            var ratings = await _repository.GetAllAsync();
            return Ok(ratings);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetByIdAsync(int id)
        {
            var rating = await _repository.GetByIdAsync(id);
            if (rating == null)
            {
                return NotFound();
            }
            return rating;
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> CreateAsync([FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(rating);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = rating.Id }, rating);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, RatingDto ratingDto)
        {
            if (id != ratingDto.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalRating = await _repository.GetByIdAsync(id);
                if (originalRating == null)
                {
                    return NotFound();
                }
                originalRating.MoodyRating = ratingDto.MoodysRating;
                originalRating.SandPRating = ratingDto.SandPRating;
                originalRating.FitchRating = ratingDto.FitchRating;
                originalRating.OrderNumber = ratingDto.OrderNumber;

                await _repository.UpdateAsync(originalRating);
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
