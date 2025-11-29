using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Models;
using kokos.Api.Models.Types;

namespace kokos.Api.Mappings
{
	public class EventInfoWithNamesOnlyMappingProfile : Profile
	{
		public EventInfoWithNamesOnlyMappingProfile()
		{
			// 1.Tell AutoMapper how to convert the User Entity to the Sub-DTO
			// Since property names (Id, Login) match exactly, no extra config is needed.
			CreateMap<UserSimple, UserIdLogin>();

			// 2. Tell AutoMapper how to convert the Event Entity to the Main DTO
			// AutoMapper will automatically use the map above for 'Organizator' and the Lists!
			CreateMap<EventInfo, EventInfoWithNamesOnlyDTO>()
				.ForMember(dest => dest.Uczestnicy, opt => opt.MapFrom(src =>
				// Combine both lists, handling nulls
				(src.UczestnicyPotwierdzeni.Select(u => new UserIdLoginPotwierdzony() { Potwierdzony = true, Id = u.Id, Login = u.Login }) ?? new List<UserIdLoginPotwierdzony>())
				.Concat(src.UczestnicyChetni.Select(u => new UserIdLoginPotwierdzony() { Potwierdzony = false, Id = u.Id, Login = u.Login }) ?? new List<UserIdLoginPotwierdzony>())
			));

			// If you need the reverse (DTO -> Entity) for Creating events
			CreateMap<EventInfoWithNamesOnlyDTO, EventInfo>()
				.ForMember(dest => dest.Organizator, opt => opt.Ignore()); // Usually ignore complex objects on create
		}
	}
}
