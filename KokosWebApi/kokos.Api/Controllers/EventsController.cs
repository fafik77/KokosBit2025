using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Exceptions;
using kokos.Api.Models;
using kokos.Api.Models.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kokos.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly UserEventDbContext _context;
		private readonly IMapper _mapper;

		public EventsController(UserEventDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}


		// GET: api/Events
		[HttpGet]
		public async Task<ActionResult<IEnumerable<EventInfoWithNamesOnlyDTO>>> GetWydarzenia()
		{
			var res = await _context.Wydarzenia
				.Include(e => e.Organizator)
				.Include(e => e.UczestnicyPotwierdzeni)
				.Include(e => e.UczestnicyChetni)
				.AsNoTracking().ToListAsync();
			var dto = _mapper.Map<IEnumerable<EventInfoWithNamesOnlyDTO>>(res);
			return Ok(dto);
		}

		// GET: api/Events/{}
		[HttpGet("{id}")]
		public async Task<ActionResult<EventInfoWithNamesOnlyDTO>> GetWydarzenie(long id)
		{
			var res = await _context.Wydarzenia
			.Include(e => e.Organizator)
			.Include(e => e.UczestnicyPotwierdzeni)
			.Include(e => e.UczestnicyChetni)
			.FirstOrDefaultAsync(e => e.Id == id);

			if (res == null)
			{
				return NotFound();
			}

			// AutoMapper does the work here based on your Profile
			EventInfoWithNamesOnlyDTO dto = _mapper.Map<EventInfoWithNamesOnlyDTO>(res);

			return Ok(dto);
		}

		[HttpPost]
		public async Task<ActionResult<EventInfoWithNamesOnlyDTO>> PostWydarzenie([FromBody] EventCreate eventCreate)
		{
			//if (eventCreate.OrganizatorId == null)
			//	throw new InvalidUserException("Organizator is required");

			var organizator = await _context.Uzytkownicy.FindAsync(eventCreate.OrganizatorId);
			if (organizator == null)
				throw new InvalidUserException($"User id {eventCreate.OrganizatorId} does not exits");

			EventInfo evInfo = new()
			{
				Nazwa = eventCreate.Nazwa,
				Data = eventCreate.Data,
				Godzina = eventCreate.Godzina,
				Opis = eventCreate.Opis,
				Szerokosc = eventCreate.Szerokosc,
				Typ = eventCreate.Typ,
				Wysokosc = eventCreate.Wysokosc,
				Organizator = organizator,
				// Initialize lists to avoid null reference issues later
				UczestnicyPotwierdzeni = new List<UserSimple>(),
				UczestnicyChetni = new List<UserSimple>()
			};

			_context.Wydarzenia.Add(evInfo);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				throw;
			}
			// FIX: Use AutoMapper here to turn the fully created Entity into the DTO
			// This ensures the Organizator string is populated correctly in the response
			EventInfoWithNamesOnlyDTO dto = _mapper.Map<EventInfoWithNamesOnlyDTO>(evInfo);

			return CreatedAtAction(nameof(GetWydarzenie), new { id = dto.Id }, dto);
		}

		[HttpPost("{evtId}/adduser/{usrId}")]
		public async Task<ActionResult<EventInfoWithNamesOnlyDTO>> AddUserToEvent(long evtId, int usrId)
		{
			//if (eventCreate.OrganizatorId == null)
			//	throw new InvalidUserException("Organizator is required");

			var userJoining = await _context.Uzytkownicy.FindAsync(usrId);
			if (userJoining == null)
				throw new InvalidUserException($"User id {usrId} does not exits");

			var eventToJoin = await _context.Wydarzenia
				.Include(e => e.Organizator)
				.Include(e => e.UczestnicyPotwierdzeni)
				.Include(e => e.UczestnicyChetni)
				.FirstOrDefaultAsync(e => e.Id == evtId);
			if (eventToJoin == null)
				throw new InvalidEventException($"Event id {evtId} does not exits");

			if (eventToJoin.Zakonczone)
				return BadRequest($"Event id {evtId} has concluded");

			if (eventToJoin.UczestnicyChetni == null) eventToJoin.UczestnicyChetni = new();

			if (userJoining == eventToJoin.Organizator) //Organizator can not join Event
			{
				throw new InvalidEventException("Organizator can not join Event");
			}

			var alreadyIn =
			eventToJoin.UczestnicyChetni.Find(u => u.Id == userJoining.Id);
			if (alreadyIn == null)
				alreadyIn =
				eventToJoin.UczestnicyPotwierdzeni?.Find(u => u.Id == userJoining.Id);

			if (alreadyIn != null)
				throw new UserAlreadyInEventException($"User {usrId} already is in Event {evtId}!");

			eventToJoin.UczestnicyChetni.Add(userJoining);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				throw;
			}
			// FIX: Use AutoMapper here to turn the fully created Entity into the DTO
			// This ensures the Organizator string is populated correctly in the response
			EventInfoWithNamesOnlyDTO dto = _mapper.Map<EventInfoWithNamesOnlyDTO>(eventToJoin);

			return CreatedAtAction(nameof(GetWydarzenie), new { id = dto.Id }, dto);
		}
	}
}
