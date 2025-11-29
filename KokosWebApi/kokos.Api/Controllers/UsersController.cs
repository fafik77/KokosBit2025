using AutoMapper;
using kokos.Api.Exceptions;
using kokos.Api.Models;
using kokos.Api.Models.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
		public async Task<ActionResult<IEnumerable<UserSimple>>> Get()
		{
			var res = await _context.Uzytkownicy.AsNoTracking().ToListAsync();
			return Ok(res);
		}
		public class UserLogin
		{
			public string Login { get; set; }
		}
		[HttpPost("bylogin")]
		public async Task<ActionResult<UserSimple>> GetUserByLogin(UserLogin login)
		{
			var user = await _context.Uzytkownicy.FirstOrDefaultAsync(u => u.Login == login.Login);
			if (user == null) return NotFound();
			return Ok(user);
		}

		// GET api/<UsersController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<UserSimple>> GetUser(int id)
		{
			var user = await _context.Uzytkownicy.FindAsync(id);
			if (user == null) return NotFound();

			return Ok(user);
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
