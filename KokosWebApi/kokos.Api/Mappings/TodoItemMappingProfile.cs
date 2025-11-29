using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Models;

namespace kokos.Api.Mappings
{
	public class TodoItemMappingProfile : Profile
	{
		public TodoItemMappingProfile()
		{
			CreateMap<TodoItem, TodoItemDTO>();
			CreateMap<TodoItemDTO, TodoItem>();
		}
	}
}
