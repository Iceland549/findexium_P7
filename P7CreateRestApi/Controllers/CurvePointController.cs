using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace P7CreateRestApi.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<CurvePoint>>> GetAllAsync()
        {
            var curvePoints = await _repository.GetAllAsync();
            return Ok(curvePoints);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAsync(int id, CurvePointDto curvePointDto)
        {
            if (id != curvePointDto.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalCurvePoint = await _repository.GetByIdAsync(id);
                if (originalCurvePoint == null)
                {
                    return NotFound();
                }
                originalCurvePoint.CurveId = curvePointDto.CurveId;
                originalCurvePoint.Term = curvePointDto.Term;
                originalCurvePoint.CurvePointValue = curvePointDto.CurvePointValue;

                await _repository.UpdateAsync(originalCurvePoint);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
