using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Models;
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
		public async Task<ActionResult<IEnumerable<TodoItem>>> GetWydarzenia()
		{
			var res = await _context.Wydarzenia.AsNoTracking().ToListAsync();
			EventInfoWithNamesOnlyDTO dto = _mapper.Map<EventInfoWithNamesOnlyDTO>(res);
			return Ok(dto);
		}

	}
}
