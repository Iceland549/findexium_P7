using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using P7CreateRestApi.Dtos;
using Microsoft.AspNetCore.Authorization;


namespace P7CreateRestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RuleNameController : ControllerBase
    {
        private readonly RuleNameRepository _repository;

        public RuleNameController(RuleNameRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<RuleName>>> GetAllAsync()
        {
            var ruleNames = await _repository.GetAllAsync();
            return Ok(ruleNames);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<RuleName>> GetByIdAsync(int id)
        {
            var ruleName = await _repository.GetByIdAsync(id);
            if (ruleName == null)
            {
                return NotFound();
            }
            return ruleName;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<RuleName>> CreateAsync([FromBody] RuleName ruleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(ruleName);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = ruleName.Id }, ruleName);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] RuleNameDto ruleNameDto)
        {
            if (id != ruleNameDto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var originalRuleName = await _repository.GetByIdAsync(id);
                if (originalRuleName == null)
                {
                    return NotFound();
                }
                originalRuleName.Name = ruleNameDto.Name;
                originalRuleName.Description = ruleNameDto.Description;
                originalRuleName.Json = ruleNameDto.Json;
                originalRuleName.Template = ruleNameDto.Template;
                originalRuleName.SqlStr = ruleNameDto.SqlStr;
                originalRuleName.SqlPart = ruleNameDto.SqlPart;

                await _repository.UpdateAsync(originalRuleName);
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
