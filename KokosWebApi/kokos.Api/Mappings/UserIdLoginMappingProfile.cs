using AutoMapper;
using kokos.Api.DTO.Types;
using kokos.Api.Models;

namespace kokos.Api.Mappings
{
	public class UserIdLoginMappingProfile : Profile
	{
		public UserIdLoginMappingProfile()
		{
			CreateMap<UserIdLogin, User>();
			CreateMap<User, UserIdLogin>();
		}
	}
}
