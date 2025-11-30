using AutoMapper;
using AutoMapper.QueryableExtensions;
using kokos.Api.DTO;
using kokos.Api.DTO.Types;
using kokos.Api.Exceptions;
using kokos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kokos.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly UserEventDbContext _context;
		private readonly IMapper _mapper;
		public UsersController(UserEventDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}
		// GET: api/<UsersController>
		[HttpGet]
		public async Task<ActionResult<List<UserSimpleDto>>> GetAllUsers()
		{
			// No .Include() needed here!
			// Entity Framework + AutoMapper generates the optimized SQL automatically
			var dtos = await _context.Uzytkownicy
				.ProjectTo<UserSimpleDto>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return Ok(dtos);
		}
		public class UserLogin
		{
			public string Login { get; set; }
		}
		[HttpPost("bylogin")]
		public async Task<ActionResult<UserSimpleDto>> GetUserByLogin(UserLogin login)
		{
			var user = await _context.Uzytkownicy
				.Include(u => u.Wydarzenia)       // Load events to count them
				.Include(u => u.OpinionsForUser)  // Load opinions to average them
				.FirstOrDefaultAsync(u => u.Login == login.Login);

			if (user == null) return NotFound();

			var dto = _mapper.Map<UserSimpleDto>(user);
			return Ok(dto);
		}

		// GET api/<UsersController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<UserSimpleDto>> GetUser(int id)
		{
			var user = await _context.Uzytkownicy
				.Include(u => u.Wydarzenia)       // Load events to count them
				.Include(u => u.OpinionsForUser)  // Load opinions to average them
				.FirstOrDefaultAsync(u => u.Id == id);

			if (user == null) return NotFound();

			var dto = _mapper.Map<UserSimpleDto>(user);
			return Ok(dto);
		}

		// POST api/<UsersController>
		[HttpPost]
		public async Task<ActionResult<UserIdLoginPreferencje>> Post([FromBody] UserLoginPreferencje userLoginPreferencje)
		{
			UserSimple user = new UserSimple() { Login = userLoginPreferencje.Login, Preferencje = userLoginPreferencje.Preferencje };
			_context.Uzytkownicy.Add(user);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				throw new UserAlreadyExistsException($"User with login: '{userLoginPreferencje.Login}' already exists");
			}
			UserIdLoginPreferencje dto = new() { Id = user.Id, Login = user.Login, Preferencje = user.Preferencje };
			return CreatedAtAction(nameof(GetUser), new { id = dto.Id }, dto);
		}

		public class DodajOpinieCommand
		{
			public string Komentarz { get; set; }

			// 1. Enforce Range [1-10]
			[Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
			public int Rating { get; set; }

			// 2. The Author (Who wrote the review)
			// We don't necessarily need a list of "WrittenReviews" in UserSimple, so we don't use InverseProperty here.
			public int AutorId { get; set; }
		}

		// POST api/<UsersController>/{}/opinie
		[HttpPost("{id}/opinie")]
		public async Task<IActionResult> DodajOpinie(int id, [FromBody] DodajOpinieCommand dodajOpinie)
		{
			var user = await _context.Uzytkownicy
				//.Include(u => u.Wydarzenia)       // Load events to count them
				.Include(u => u.OpinionsForUser)  // Load opinions to add a new one
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
				throw new InvalidUserException($"User id {id} does not exits");

			var userAuthor = await _context.Uzytkownicy
				.FirstOrDefaultAsync(u => u.Id == dodajOpinie.AutorId);

			if (userAuthor == null)
				throw new InvalidUserException($"User id {id} does not exits");

			if (user.OpinionsForUser == null) user.OpinionsForUser = new();

			var nowaOpinia = new OpinionsForUser()
			{
				Autor = userAuthor,
				Komentarz = dodajOpinie.Komentarz,
				Rating = dodajOpinie.Rating,
				RatedUser = user,
				UserId = user.Id
			};

			user.OpinionsForUser.Add(nowaOpinia);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				throw;// new UserAlreadyExistsException($"User with login: '{userLoginPreferencje.Login}' already exists");
			}
			return NoContent();
		}

		// get api/<UsersController>/{}/opinie
		[HttpGet("{id}/opinie")]
		public async Task<ActionResult<IEnumerable<OpinionsForUserDto>>> GetOpinieDla(int id)
		{
			var user = await _context.Uzytkownicy
				.Include(u => u.OpinionsForUser)  // Load opinions
					.ThenInclude(o => o.Autor) // <--- THIS IS REQUIRED
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
				throw new InvalidUserException($"User id {id} does not exits");

			var dto = _mapper.Map<IEnumerable<OpinionsForUserDto>>(user.OpinionsForUser);
			return Ok(dto);
		}

		[HttpGet("{id}/eventyOrganizuje")]
		public async Task<ActionResult<IEnumerable<EventInfoWithNamesOnlyDTO>>> GetEventsOrganized(int id)
		{
			var user = await _context.Uzytkownicy
				.Include(u => u.Wydarzenia)
					.ThenInclude(e => e.UczestnicyChetni)      // 1. Load Willing
				.Include(u => u.Wydarzenia)                    // <--- Go back to "Wydarzenia"
					.ThenInclude(e => e.UczestnicyPotwierdzeni)// 2. Load Confirmed
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Id == id);
			if (user == null)
				throw new InvalidUserException($"User id {id} does not exits");

			var dto = _mapper.Map<IEnumerable<EventInfoWithNamesOnlyDTO>>(user.Wydarzenia);
			return Ok(dto);
		}

		[HttpGet("{id}/eventyPotwierdzony")]
		public async Task<ActionResult<IEnumerable<EventInfoWithNamesOnlyDTO>>> GetEventsParticipatesConfirmed(int id)
		{
			var userExists = await _context.Uzytkownicy.AnyAsync(u => u.Id == id);
			if (!userExists)
			{
				// Assuming InvalidUserException is your custom exception
				throw new InvalidUserException($"User id {id} does not exist");
			}

			// 2. Query the events
			var eventsParticipating = await _context.Wydarzenia
				.Include(e => e.Organizator)              // Necessary for DTO mapping
				.Include(e => e.UczestnicyPotwierdzeni)   // Necessary for filtering AND DTO
				.Include(e => e.UczestnicyChetni)         // Necessary for DTO mapping
														  // Logic: "Select events where Confirmed Participants list contains this ID"
				.Where(e => 
				e.UczestnicyPotwierdzeni.Any(u => u.Id == id)
				)
				.AsNoTracking()
				.ToListAsync();

			var dto = _mapper.Map<IEnumerable<EventInfoWithNamesOnlyDTO>>(eventsParticipating);
			return Ok(dto);
		}

		[HttpGet("{id}/eventyOczekuje")]
		public async Task<ActionResult<IEnumerable<EventInfoWithNamesOnlyDTO>>> GetEventsParticipatesPending(int id)
		{
			var userExists = await _context.Uzytkownicy.AnyAsync(u => u.Id == id);
			if (!userExists)
			{
				// Assuming InvalidUserException is your custom exception
				throw new InvalidUserException($"User id {id} does not exist");
			}

			// 2. Query the events
			var eventsParticipating = await _context.Wydarzenia
				.Include(e => e.Organizator)              // Necessary for DTO mapping
				.Include(e => e.UczestnicyPotwierdzeni)   // Necessary for filtering AND DTO
				.Include(e => e.UczestnicyChetni)         // Necessary for DTO mapping
														  // Logic: "Select events where Confirmed Participants list contains this ID"
				.Where(e => 
				e.UczestnicyChetni.Any(u => u.Id == id)
				)
				.AsNoTracking()
				.ToListAsync();

			var dto = _mapper.Map<IEnumerable<EventInfoWithNamesOnlyDTO>>(eventsParticipating);
			return Ok(dto);
		}

		[HttpGet("{id}/eventyZapisany")]
		public async Task<ActionResult<IEnumerable<EventInfoWithNamesOnlyDTO>>> GetEventsParticipatesAny(int id)
		{
			var userExists = await _context.Uzytkownicy.AnyAsync(u => u.Id == id);
			if (!userExists)
			{
				// Assuming InvalidUserException is your custom exception
				throw new InvalidUserException($"User id {id} does not exist");
			}

			// 2. Query the events
			var eventsParticipating = await _context.Wydarzenia
				.Include(e => e.Organizator)              // Necessary for DTO mapping
				.Include(e => e.UczestnicyPotwierdzeni)   // Necessary for filtering AND DTO
				.Include(e => e.UczestnicyChetni)         // Necessary for DTO mapping
														  // Logic: "Select events where Confirmed Participants list contains this ID"
				.Where(e => 
					e.UczestnicyPotwierdzeni.Any(u => u.Id == id) ||
					e.UczestnicyChetni.Any(u => u.Id == id)
				)
				.AsNoTracking()
				.ToListAsync();

			var dto = _mapper.Map<IEnumerable<EventInfoWithNamesOnlyDTO>>(eventsParticipating);
			return Ok(dto);
		}

		// PUT api/Users/{}
		// updates only preferences, as Login has to be unique
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] UserLoginPreferencje userLoginPreferencje)
		{
			var user = await _context.Uzytkownicy.FindAsync(id);
			if (user == null) return NotFound();
			user.Preferencje = userLoginPreferencje.Preferencje;
			_context.Entry(user).State = EntityState.Modified;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE api/Users/{}
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var user = await _context.Uzytkownicy.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Uzytkownicy.Remove(user);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
