using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace kokos.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodosController : ControllerBase
	{
		private readonly TodoDBContext _context;
		private readonly IMapper _mapper;

		public TodosController(TodoDBContext context, IMapper mapper)
		{
			_context = context;
			this._mapper = mapper;
		}

		// GET: api/TodoItems
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
		{
			var res = await _context.TodoItems.AsNoTracking().ToListAsync();
			return Ok(res);
		}

		// GET: api/TodoItems/5
		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
		{
			var todoItem = await _context.TodoItems.FindAsync(id);
			if (todoItem == null) return NotFound();
			var todoItemDto = _mapper.Map<TodoItemDTO>(todoItem);
			return Ok(todoItemDto);
		}

		// PUT: api/TodoItems/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
		{
			if (id != todoItem.Id)
			{
				return BadRequest();
			}

			_context.Entry(todoItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TodoItemExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/TodoItems
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItemDTO todoItem)
		{
			TodoItem todoItem1 = _mapper.Map<TodoItem>(todoItem);
			_context.TodoItems.Add(todoItem1);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem1.Id }, todoItem1);
		}

		// DELETE: api/TodoItems/5
		[HttpDelete("{id}")]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> DeleteTodoItem(long id)
		{
			var todoItem = await _context.TodoItems.FindAsync(id);
			if (todoItem == null)
			{
				return NotFound();
			}

			_context.TodoItems.Remove(todoItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}


		private bool TodoItemExists(long id)
		{
			return _context.TodoItems.Any(e => e.Id == id);
		}
	}
}
