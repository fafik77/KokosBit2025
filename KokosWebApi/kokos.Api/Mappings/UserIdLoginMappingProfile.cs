using AutoMapper;
using kokos.Api.Models;
using kokos.Api.Models.Types;

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
