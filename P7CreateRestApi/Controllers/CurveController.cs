using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurvePointController : ControllerBase
    {
        private readonly CurvePointRepository _repository;

        public CurvePointController(CurvePointRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurvePoint>>> GetAllAsync()
        {
            var curvePoints = await _repository.GetAllAsync();
            return Ok(curvePoints);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePoint>> GetByIdAsync(int id)
        {
            var curvePoint = await _repository.GetByIdAsync(id);
            if (curvePoint == null)
            {
                return NotFound();
            }
            return curvePoint;
        }

        [HttpPost]
        public async Task<ActionResult<CurvePoint>> CreateAsync([FromBody] CurvePoint curvePoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(curvePoint);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = curvePoint.Id }, curvePoint);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, CurvePoint curvePoint)
        {
            if (id != curvePoint.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _repository.UpdateAsync(curvePoint);
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
