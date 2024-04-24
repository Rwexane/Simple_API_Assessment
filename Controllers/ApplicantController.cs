using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Simple_API_Assessment.Data.Repository;
using SimpleA.Models;

namespace Simple_API_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantRepository _applicantRepository;

        public ApplicantController(IApplicantRepository applicantRepository)
        {
            _applicantRepository = applicantRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Applicant>>> GetAllApplicants()
        {
            var applicants = await _applicantRepository.GetAllApplicantsAsync();
            return Ok(applicants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> GetApplicantById(int id)
        {
            var applicant = await _applicantRepository.GetApplicantByIdAsync(id);
            if (applicant == null)
            {
                return NotFound($"Applicant with ID {id} not found.");
            }
            return Ok(applicant);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddApplicant(Applicant applicant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newApplicantId = await _applicantRepository.AddApplicantAsync(applicant);
            return CreatedAtAction(nameof(GetApplicantById), new { id = newApplicantId }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplicant(int id, Applicant applicant)
        {
            if (id != applicant.Id)
            {
                return BadRequest("Applicant ID mismatch.");
            }

            await _applicantRepository.UpdateApplicantAsync(applicant);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicant(int id)
        {
            var applicant = await _applicantRepository.GetApplicantByIdAsync(id);
            if (applicant == null)
            {
                return NotFound($"Applicant with ID {id} not found.");
            }

            await _applicantRepository.DeleteApplicantAsync(id);
            return NoContent();
        }
    }
}
