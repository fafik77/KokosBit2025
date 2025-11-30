using AutoMapper;
using kokos.Api.DTO;
using kokos.Api.DTO.Types;
using kokos.Api.Models;

namespace kokos.Api.Mappings
{
	public class OpinionsForUserMappingProfile : Profile
	{
		public OpinionsForUserMappingProfile()
		{
			// 1. Define the Nested Mapping (UserSimple -> UserIdLogin)
			// AutoMapper needs this to know how to convert the "Autor" property.
			CreateMap<UserSimple, UserIdLogin>();

			// 2. Define the Main Mapping (Entity -> DTO)
			CreateMap<OpinionsForUser, OpinionsForUserDto>();

			// 3. (Optional) DTO -> Entity
			CreateMap<OpinionsForUserDto, OpinionsForUser>()
			   .ForMember(dest => dest.Autor, opt => opt.Ignore())
			   .ForMember(dest => dest.RatedUser, opt => opt.Ignore());
		}
	}
}
