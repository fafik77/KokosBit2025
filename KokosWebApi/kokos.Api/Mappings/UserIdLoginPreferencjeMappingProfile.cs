using AutoMapper;
using kokos.Api.Models;
using kokos.Api.Models.Types;

namespace kokos.Api.Mappings
{
	public class UserIdLoginPreferencjeMappingProfile : Profile
	{
		public UserIdLoginPreferencjeMappingProfile()
		{
			CreateMap<UserIdLoginPreferencje, User>();
			CreateMap<User, UserIdLoginPreferencje>();
		}
	}
}
