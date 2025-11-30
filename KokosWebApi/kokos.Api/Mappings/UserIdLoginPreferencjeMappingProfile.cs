using AutoMapper;
using kokos.Api.DTO.Types;
using kokos.Api.Models;

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
