using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces.UseCases;

namespace EventsWebApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantUseCase _participantUseCase;

        public ParticipantsController(IParticipantUseCase participantUseCase)
        {
            _participantUseCase = participantUseCase;
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<ParticipantResponseDto>>> GetParticipantsByEventId(int eventId)
        {
            var participantDtos = await _participantUseCase.GetParticipantsByEventIdAsync(eventId);
            return Ok(participantDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetParticipantById(int id)
        {
            var participantDto = await _participantUseCase.GetParticipantByIdAsync(id, User);
            return Ok(participantDto);
        }

        [HttpPost("register/{eventId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult> RegisterParticipant(int eventId)
        {
            await _participantUseCase.RegisterParticipantAsync(eventId, User);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] ParticipantRequestDto participantDto)
        {
            await _participantUseCase.UpdateParticipantAsync(id, participantDto, User);
            return NoContent();
        }

        [HttpDelete("remove/{eventId}/{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult> DeleteParticipantFromEvent(int eventId, int id)
        {
            await _participantUseCase.DeleteParticipantFromEventAsync(eventId, id, User);
            return NoContent();
        }
    }
}
