using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.Models;

namespace kokos.Api.Mappings
{
	public class UserSimpleMappingProfile : Profile
	{
		public UserSimpleMappingProfile()
		{
			CreateMap<UserSimple, UserSimpleDto>()
				// 1. Calculate 'TrwajaceWydarzenia'
				// We count events where the list is not null AND Zakonczone is false
				.ForMember(dest => dest.TrwajaceWydarzenia, opt => opt.MapFrom(src =>
					src.Wydarzenia != null
					? src.Wydarzenia.Count(e => !e.Zakonczone)
					: 0))

				// 2. Calculate 'UserRating'
				// We calculate Average only if list exists and has items (to avoid DivideByZero exception)
				.ForMember(dest => dest.UserRating, opt => opt.MapFrom(src =>
					src.OpinionsForUser != null && src.OpinionsForUser.Any()
					? (float)src.OpinionsForUser.Average(o => o.Rating)
					: 0.0f));
		}
	}
}
