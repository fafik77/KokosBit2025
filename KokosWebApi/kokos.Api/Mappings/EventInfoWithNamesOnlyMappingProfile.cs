using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Models;

namespace kokos.Api.Mappings
{
	public class EventInfoWithNamesOnlyMappingProfile : Profile
	{
		public EventInfoWithNamesOnlyMappingProfile()
		{
			CreateMap<EventInfoWithNamesOnlyDTO, EventInfo>();
			CreateMap<EventInfo, EventInfoWithNamesOnlyDTO>();
		}
	}
}
