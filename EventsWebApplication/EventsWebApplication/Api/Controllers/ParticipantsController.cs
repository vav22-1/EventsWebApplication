using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParticipantsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipantsByEventId(int eventId)
        {
            var participants = await _unitOfWork.Participants.GetParticipantsByEventIdAsync(eventId);
            var participantDtos = _mapper.Map<IEnumerable<ParticipantResponseDto>>(participants);
            return Ok(participantDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetParticipantById(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null || user.ParticipantId != id)
            {
                return Forbid("Нет доступа к данным этого участника.");
            }
            var participantItem = await _unitOfWork.Participants.GetParticipantByIdAsync(id);
            if (participantItem == null) return NotFound();

            var participantDto = _mapper.Map<ParticipantResponseDto>(participantItem);
            return Ok(participantDto);
        }

        [HttpPost("register/{eventId}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult> RegisterParticipant(int eventId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null || user.ParticipantId == null)
            {
                return NotFound("Пользователь не найден или нет участника.");
            }
            var participantId = user.ParticipantId;
            await _unitOfWork.Participants.AddParticipantToEventAsync(eventId, (int)participantId);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> UpdateParticipant(int id, [FromBody] ParticipantRequestDto participantDto)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null || user.ParticipantId != id)
            {
                return Forbid("Нет доступа к данным этого участника.");
            }

            var participant = await _unitOfWork.Participants.GetParticipantByIdAsync(id);
            if (participant == null) return NotFound();

            _mapper.Map(participantDto, participant);
            await _unitOfWork.Participants.UpdateParticipantAsync(participant);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("remove/{eventId}/{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult> DeleteParticipantFromEvent(int eventId, int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "0");
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null || user.ParticipantId != id)
            {
                return Forbid("Нет доступа к данным этого участника.");
            }

            await _unitOfWork.Participants.RemoveParticipantFromEventAsync(eventId, id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
