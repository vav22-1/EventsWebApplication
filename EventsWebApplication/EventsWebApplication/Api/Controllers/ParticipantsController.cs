using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsWebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public ParticipantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipantsByEventId(int eventId)
        {
            var participants = await _unitOfWork.Participants.GetParticipantsByEventIdAsync(eventId);
            return Ok(participants);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetParticipantById(int id)
        {
            var participantItem = await _unitOfWork.Participants.GetParticipantByIdAsync(id);
            if (participantItem == null) return NotFound();
            return Ok(participantItem);
        }

        [HttpPost]
        public async Task<ActionResult> RegisterParticipant([FromBody] ParticipantDto participantDto)
        {
            var participant = new Participant
            {
                FirstName = participantDto.FirstName,
                LastName = participantDto.LastName,
                DateOfBirth = participantDto.DateOfBirth,
                Email = participantDto.Email,
                EventId = participantDto.EventId,
                DateOfRegistration = DateTime.UtcNow
            };
            await _unitOfWork.Participants.AddParticipantAsync(participant);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetParticipantById), new { id = participant.Id }, participant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParticipant(int id)
        {
            await _unitOfWork.Participants.DeleteParticipantAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
