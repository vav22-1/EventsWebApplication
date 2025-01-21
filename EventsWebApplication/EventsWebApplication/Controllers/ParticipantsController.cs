using EventsWebApplication.Application.DTOs.ParticipantDTOs;
using EventsWebApplication.Core.Interfaces;

namespace EventsWebApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IUseCase<GetParticipantsByEventIdRequestDto, IEnumerable<ParticipantResponseDto>> _getParticipantsByEventIdUseCase;
        private readonly IUseCase<GetParticipantByIdRequestDto, ParticipantResponseDto> _getParticipantByIdUseCase;
        private readonly IUseCase<RegisterParticipantRequestDto, Unit> _registerParticipantUseCase;
        private readonly IUseCase<UpdateParticipantRequestDto, Unit> _updateParticipantUseCase;
        private readonly IUseCase<DeleteParticipantFromEventRequestDto, Unit> _deleteParticipantFromEventUseCase;

        public ParticipantsController(
            IUseCase<GetParticipantsByEventIdRequestDto, IEnumerable<ParticipantResponseDto>> getParticipantsByEventIdUseCase,
            IUseCase<GetParticipantByIdRequestDto, ParticipantResponseDto> getParticipantByIdUseCase,
            IUseCase<RegisterParticipantRequestDto, Unit> registerParticipantUseCase,
            IUseCase<UpdateParticipantRequestDto, Unit> updateParticipantUseCase,
            IUseCase<DeleteParticipantFromEventRequestDto, Unit> deleteParticipantFromEventUseCase)
        {
            _getParticipantsByEventIdUseCase = getParticipantsByEventIdUseCase;
            _getParticipantByIdUseCase = getParticipantByIdUseCase;
            _registerParticipantUseCase = registerParticipantUseCase;
            _updateParticipantUseCase = updateParticipantUseCase;
            _deleteParticipantFromEventUseCase = deleteParticipantFromEventUseCase;
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<ParticipantResponseDto>>> GetParticipantsByEventId(int eventId)
        {
            var requestDto = new GetParticipantsByEventIdRequestDto { EventId = eventId };
            var participantDtos = await _getParticipantsByEventIdUseCase.ExecuteAsync(requestDto);
            return Ok(participantDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetParticipantById(int id)
        {
            var requestDto = new GetParticipantByIdRequestDto { Id = id, User = User };
            var participantDto = await _getParticipantByIdUseCase.ExecuteAsync(requestDto);
            return Ok(participantDto);
        }

        [HttpPost("register/{eventId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult> RegisterParticipant(int eventId)
        {
            var requestDto = new RegisterParticipantRequestDto { EventId = eventId, User = User };
            await _registerParticipantUseCase.ExecuteAsync(requestDto);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] ParticipantRequestDto participantDto)
        {
            var requestDto = new UpdateParticipantRequestDto { Id = id, ParticipantDto = participantDto, User = User };
            await _updateParticipantUseCase.ExecuteAsync(requestDto);
            return NoContent();
        }

        [HttpDelete("remove/{eventId}/{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult> DeleteParticipantFromEvent(int eventId, int id)
        {
            var requestDto = new DeleteParticipantFromEventRequestDto { EventId = eventId, ParticipantId = id, User = User };
            await _deleteParticipantFromEventUseCase.ExecuteAsync(requestDto);
            return NoContent();
        }
    }
}
